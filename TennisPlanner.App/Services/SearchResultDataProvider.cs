using Radzen;
using TennisPlanner.Core.Clients;
using TennisPlanner.Core.Contracts;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Exceptions;
using TennisPlanner.Shared.Models;
using TennisPlanner.Shared.Services.Logging;

namespace TennisPlanner.App.Services;

/// <inheritdoc/>
public class SearchResultDataProvider : ISearchResultDataProvider
{
    private DateTime? _dateTime;

    public event EventHandler<ResultsChangedArgs> ResultsChanged;

    private readonly ITennisPlannerAPIService _apiService;
    private readonly ISearchFiltersService _searchFiltersService;
    private readonly ILoggerService _loggerService;
    private readonly NotificationService _notificationService;

    public SearchResultDataProvider(
        ITennisPlannerAPIService apiService,
        ISearchFiltersService searchFiltersService,
        ILoggerService loggerService,
        NotificationService notificationService)
    {
        _apiService = apiService;
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
        _dateTime = dateTime;
        try
        {
            var data = await _apiService.GetTennisDataAsync(day: dateTime);
            await FillSearchResultsAsync(data);
        }
        catch (ApiException)
        {
            Results = new SearchResults(errorCode: SearchFailureErrorCode.ApiFailure);
            ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
        }
        catch (Exception ex)
        {
            Results = new SearchResults(errorCode: SearchFailureErrorCode.Unknown);
            ResultsChanged.Invoke(this, new ResultsChangedArgs(Results));
        }
    }

    /// <inheritdoc/>
    public void AddPlayerLocation(AddressModel address)
    {
        var gc = address.Value?.GeoCoordinates;

        if (Results.SearchResultItems == null || Results.FromGeoCoordinates == null || gc == null)
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

    private async Task<Journey?> GetTransportationJourneyAsync(SearchResultItem item, GeoCoordinates gc)
    {
        try
        {
            var value = await _apiService.GetTransportationJourneyAsync(
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
        AddPlayerLocation(address: address);
    }
    private void SearchFiltersService_RemovePlayerAddressEvent(object? sender, AddressModel address)
    {
        RemovePlayerLocation(address: address);
    }

    private void SearchFiltersService_AddHourRangeEvent(object? sender, HourRangeSelectorModel hourRangeSelectorModel)
    {
        AddHourRange(hourRangeSelectorModel: hourRangeSelectorModel);
    }

    private void SearchFiltersService_RemoveHourRangeEvent(object? sender, HourRangeSelectorModel hourRangeSelectorModel)
    {
        RemoveHourRange(hourRangeSelectorModel: hourRangeSelectorModel);
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
