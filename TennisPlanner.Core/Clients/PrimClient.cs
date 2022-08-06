using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Extensions;
using TennisPlanner.Shared.Models;
using TennisPlanner.Shared.Services.Logging;

namespace TennisPlanner.Core.Clients;

/// <inheritdoc/>
public class PrimClient : ITransportClient
{
    const string baseApiUrl = "https://prim.iledefrance-mobilites.fr/marketplace/navitia/coverage/fr-idf/";
    const string journeyPath = "journeys";

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Instanciates <see cref="PrimClient"./>
    /// </summary>
    public PrimClient(string apiToken)
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(baseApiUrl),
        };
        _httpClient.DefaultRequestHeaders.Add("apikey", apiToken);
    }

    /// <inheritdoc/>
    public async Task<Journey?> GetTransportationJourneyAsync(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates)
    {
        var requestMessage = this.craftQuery(
            arrivalTime: arrivalTime,
            fromGeoCoordinates: fromGeoCoordinates,
            toGeoCoordinates: toGeoCoordinates);

        var journeyApiResult = await ApiCallAsync<IdfMobiliteJourneyDto>(requestMessage);

        return journeyApiResult?.Journeys.FirstOrDefault()?.ToJourney();
    }

    public async Task<T?> ApiCallAsync<T>(HttpRequestMessage requestMessage)
    {
        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync
                <T>(responseStream);
        }

        LoggerService.Instance.Log(
                logLevel: LogLevel.Error,
                operationName: $"{nameof(GeoClient)}.{nameof(this.ApiCallAsync)}",
                message: $"Cannot fetch data from PRIM API. Status code: {response.StatusCode}.");
        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                LoggerService.Instance.Log(
                    logLevel: LogLevel.Error,
                    operationName: $"{nameof(GeoClient)}.{nameof(this.ApiCallAsync)}",
                    message: $"Prim token is invalid. Status code: {response.StatusCode}.");
                break;
            case HttpStatusCode.TooManyRequests:
                LoggerService.Instance.Log(
                    logLevel: LogLevel.Error,
                    operationName: $"{nameof(GeoClient)}.{nameof(this.ApiCallAsync)}",
                    message: "Too many prim requests.");
                break;
            default:
                break;
        }

        return default(T?);
    }

    private HttpRequestMessage craftQuery(
        DateTime arrivalTime, 
        GeoCoordinates fromGeoCoordinates, 
        GeoCoordinates toGeoCoordinates)
    {
        var param = new Dictionary<string, string>() { 
            { "from", fromGeoCoordinates.ToString() },
            { "to", toGeoCoordinates.ToString() },
            { "datetime", arrivalTime.ToString("yyyyMMddHHmmss") },
            { "first_section_mode[]", "walking" },
            { "last_section_mode[]", "walking" },
            { "forbidden_uris[]", 
                "physical_mode:Air,"
                + "physical_mode:Boat,"
                + "physical_mode:Ferry," 
                + "physical_mode:Taxi" },
            { "count", "1" }
        };

        var requestUri = QueryHelpers.AddQueryString(journeyPath, param);
        return new HttpRequestMessage(method: HttpMethod.Get, requestUri: requestUri);
    }
}
