using Radzen;
using TennisPlanner.Core.Clients;
using TennisPlanner.Core.Contracts;
using TennisPlanner.Core.Contracts.Location;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Core.Extensions;
using TennisPlanner.Server.Models;
using TennisPlanner.Shared.Exceptions;
using TennisPlanner.Shared.Services.Logging;

namespace TennisPlanner.Server.Services;

/// <inheritdoc/>
public class SearchResultDataProvider : ISearchResultDataProvider
{
    private DateTime? _dateTime;

    public event EventHandler<ResultsChangedArgs> ResultsChanged;

    private readonly ITennisClient _tennisClient;
    private readonly ITransportClient _transportClient;
    private readonly ISearchFiltersService _searchFiltersService;
    private readonly ILoggerService _loggerService;
    private readonly NotificationService _notificationService;

    public SearchResultDataProvider(
        ITennisClient tennisClient, 
        ITransportClient transportClient, 
        ISearchFiltersService searchFiltersService,
        ILoggerService loggerService,
        NotificationService notificationService)
    {
        _tennisClient = tennisClient;
        _transportClient = transportClient;
        _searchFiltersService = searchFiltersService;
        _loggerService = loggerService;
        _notificationService = notificationService;

        searchFiltersService.AddHourRangeEvent += SearchFiltersService_AddHourRangeEvent;
        searchFiltersService.RemoveHourRangeEvent += SearchFiltersService_RemoveHourRangeEvent;
        searchFiltersService.AddPlayerAddressEvent += SearchFiltersService_AddPlayerAddressEvent;
        searchFiltersService.RemovePlayerAddressEvent += SearchFiltersService_RemovePlayerAddressEvent;
    }

    public SearchResults Results { get; private set; } = new SearchResults();

    /// <inheritdoc/>
    public SearchResults GetSearchResults()
    {
        return Results;
    }

    /// <inheritdoc/>
    public async Task Init(DateTime dateTime)
    {
        this._dateTime = dateTime;
        try
        {
            var data = await GetTennisDataAsync();
            await FillSearchResultsAsync(data);
        }
        catch (ApiException)
        {
            Results = new SearchResults(errorCode: SearchFailureErrorCode.ApiFailure);
            ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
        }
        catch (Exception)
        {
            Results = new SearchResults(errorCode: SearchFailureErrorCode.Unknown);
            ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
        }
    }

    /// <inheritdoc/>
    public void AddPlayerLocation(AddressModel address)
    {
        var gc = address.Value?.GeoCoordinates;

        if (Results.SearchResultItems == null || Results.FromGeoCoordinates == null)
        {
            throw new PlatformException();
        }

        Results.SearchResultItems.ForEach(item =>
        {
            item.journeyDurationsTasks.Add(key: gc, value: GetTransportationJourneyAsync(item, gc));
        });
        Results.FromGeoCoordinates.Add(gc);
        ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
    }

    private async Task<JourneyDurationDto?> GetTransportationJourneyAsync(SearchResultItem item, GeoCoordinates? gc)
    {
        try
        {
            var value = await _transportClient.GetTransportationJourneyAsync(
                arrivalTime: item.FromDateTime,
                fromGeoCoordinates: gc,
                toGeoCoordinates: item.CourtGeoCoordinates);
            return value;
        }
        catch
        {
            _notificationService.Notify(NotificationSeverity.Warning, summary: "Erreur API Transports");
        }
        finally
        {
            ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
        }

        return null;
    }

    private void AddHourRange(HourRangeSelectorModel hourRangeSelectorModel)
    {
        ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
    }

    /// <inheritdoc/>
    public void RemovePlayerLocation(AddressModel address)
    {
        var gc = address.Value?.GeoCoordinates;

        if (Results.SearchResultItems == null)
        {
            _loggerService.Log(
                logLevel: LogLevel.Warning,
                operationName: $"{nameof(SearchResultDataProvider)}.{nameof(this.RemovePlayerLocation)}",
                message: "Cannot remove geocoordinates from list because the result list is empty.");
            return;
        }

        foreach (var item in Results.SearchResultItems)
        {
            if (item.journeyDurationsTasks.TryGetValue(key: gc, out var task))
            {
                task.Dispose();
            }

            item.journeyDurationsTasks.Remove(key: gc);
        }
    }

    private void RemoveHourRange(HourRangeSelectorModel hourRangeSelectorModel)
    {
        ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
    }

    private async Task<IEnumerable<TimeSlot>> GetTennisDataAsync()
    {
        var courts = await _tennisClient.GetTennisFacilitiesListAsync();
        var availabilities = await Task.WhenAll(courts.Select(async court => 
        await _tennisClient.GetTimeSlotListAsync(
            tennisCourt: court, 
            day: _dateTime ?? throw new ArgumentNullException(nameof(_dateTime)))));
        var tennisSlots = availabilities.SelectMany(x => x);
        return tennisSlots;
    }

    private async Task FillSearchResultsAsync(IEnumerable<TimeSlot> slots)
    {
        await Task.Yield();
        var searchResultItems = slots.Select(slot => new SearchResultItem(
            fromDateTime: slot.TimeRange.StartHour,
            toDateTime: slot.TimeRange.EndHour,
            courtGeoCoordinates: slot.CourtInfo.Facility.Coordinates,
            tennisFacilityName: slot.CourtInfo.Facility.Name,
            courtName: slot.CourtInfo.Title,
            courtLighting: slot.CourtInfo.Light,
            courtGround: slot.CourtInfo.Ground,
            courtRoof: slot.CourtInfo.Roof));

        Results = new SearchResults(
            searchResultItems: searchResultItems.ToList(),
            fromGeoCoordinates: new());
        ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
    }

    private void SearchFiltersService_AddPlayerAddressEvent(object? sender, AddressModel address)
    {
        this.AddPlayerLocation(address: address);
    }
    private void SearchFiltersService_RemovePlayerAddressEvent(object? sender, AddressModel address)
    {
        this.RemovePlayerLocation(address: address);
    }

    private void SearchFiltersService_AddHourRangeEvent(object? sender, HourRangeSelectorModel hourRangeSelectorModel)
    {
        this.AddHourRange(hourRangeSelectorModel: hourRangeSelectorModel);
    }

    private void SearchFiltersService_RemoveHourRangeEvent(object? sender, HourRangeSelectorModel hourRangeSelectorModel)
    {
        this.RemoveHourRange(hourRangeSelectorModel: hourRangeSelectorModel);
    }

    public IEnumerable<SearchResultItem> Filter(IEnumerable<SearchResultItem> resultItems)
    {
        if (_searchFiltersService.HourRangeList.Count == 0)
        {
            return resultItems;
        }

        return resultItems.Where(resultItem => _searchFiltersService.HourRangeList.Any(hourRange =>
        hourRange.HourRange.First() <= resultItem.FromDateTime.Hour
        && hourRange.HourRange.Skip(1).First() >= resultItem.ToDateTime.Hour));
    }
}
