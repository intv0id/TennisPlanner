using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TennisPlanner.Core.Contracts;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Exceptions;
using TennisPlanner.Shared.Helpers;

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

    public async Task<IdfMobiliteTokenDto> GetTokenAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IdfMobiliteTokenDto>("GetIdfmToken");
        return result ?? throw new ApiException("GetTokenAsync");
    }
}
