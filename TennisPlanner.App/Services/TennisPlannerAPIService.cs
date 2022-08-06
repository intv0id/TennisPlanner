using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TennisPlanner.Core.Contracts;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Exceptions;
using TennisPlanner.Shared.Helpers;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

/// <summary>
/// API Proxy class.
/// </summary>
public class TennisPlannerAPIService : ITennisPlannerAPIService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor for <see cref="TennisPlannerAPIService"/>.
    /// </summary>
    /// <param name="httpClient"></param>
    public TennisPlannerAPIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TimeSlot>> GetTennisDataAsync(DateTime day)
    {
        var searchParams = new List<KeyValuePair<string, string>>();
        searchParams.Add(new KeyValuePair<string, string>(Constants.DateTimeQueryKey, day.ToString("yyyy-MM-dd")));
        var queryString = QueryString.Create(searchParams);
        var result = await _httpClient.GetFromJsonAsync<List<TimeSlot>>($"GetTennisAvailabilities{queryString.ToUriComponent()}");
        return result ?? throw new ApiException("GetTennisAvailabilities");
    }

    /// <inheritdoc/>
    public async Task<Journey?> GetTransportationJourneyAsync(
        DateTime arrivalTime, 
        GeoCoordinates fromGeoCoordinates, 
        GeoCoordinates toGeoCoordinates)
    {
        var request = new TransportationJourneyRequestDto
        {
            ArrivalTime = arrivalTime,
            FromGeoCoordinates = fromGeoCoordinates,
            ToGeoCoordinates = toGeoCoordinates,
        };

        var result = await _httpClient.PostAsJsonAsync("GetJourney", request);
        return await result.Content.ReadFromJsonAsync<Journey>() 
            ?? throw new ApiException("GetJourney");
    }
}
