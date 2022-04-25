using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TennisPlanner.Core.Contracts;
using TennisPlanner.Shared.Exceptions;
using TennisPlanner.Shared.Helpers;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

public class TennisPlannerAPIService : ITennisPlannerAPIService
{
    private readonly HttpClient _httpClient;

    public TennisPlannerAPIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TimeSlot>> GetTennisDataAsync(DateTime day)
    {
        var searchParams = new List<KeyValuePair<string, string>>();
        searchParams.Add(new KeyValuePair<string, string>(Constants.DateTimeQueryKey, day.ToString("yyyy-MM-dd")));
        var queryString = QueryString.Create(searchParams);
        var result = await _httpClient.GetFromJsonAsync<List<TimeSlot>>($"GetTennisAvailabilities{queryString.ToUriComponent()}");
        return result ?? throw new ApiException("GetTennisAvailabilities");
    }

    public async Task<Journey?> GetTransportationJourneyAsync(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates)
    {
        var requestDto = new JourneyRequestDto
        {
            ArrivalDateTime = arrivalTime,
            FromGeoCoordinates = fromGeoCoordinates,
            ToGeoCoordinates = toGeoCoordinates,
        };
        var response = await _httpClient.PostAsJsonAsync("GetJourney", requestDto);
        var result = await JsonSerializer.DeserializeAsync<Journey?>(response.Content.ReadAsStream());
        return result ?? throw new ApiException("GetTennisAvailabilities");
    }
}
