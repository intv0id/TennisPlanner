using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TennisPlanner.Core.Configuration;
using TennisPlanner.Core.Contracts.Location;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Exceptions;
using TennisPlanner.Shared.Extensions;
using TennisPlanner.Shared.Models;
using TennisPlanner.Shared.Services.Logging;
using Timer = System.Timers.Timer;

namespace TennisPlanner.Core.Clients;

/// <inheritdoc/>
public class IdfMobilitesClient : ITransportClient
{
    const string tokenUrl = "https://as.api.iledefrance-mobilites.fr/api/oauth/token";
    const string baseApiUrl = "https://traffic.api.iledefrance-mobilites.fr/v2/mri/coverage/idfm/";
    const string journeyQuery = "journeys?";

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Instanciates <see cref="IdfMobilitesClient"./>
    /// </summary>
    public IdfMobilitesClient()
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(baseApiUrl),
        };
    }

    public async Task<IdfMobiliteTokenDto> GetTokenAsync(string clientId, string clientSecret)
    {
        HttpContent content = new FormUrlEncodedContent(
               new Dictionary<string, string>()
               {
                    { "grant_type", "client_credentials" },
                    { "scope", "read-data"},
                    { "client_id", clientId},
                    { "client_secret", clientSecret},
               }
           );
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        content.Headers.ContentType.CharSet = "UTF-8";

        using var authClient = new HttpClient();
        authClient.DefaultRequestHeaders.ExpectContinue = false;
        HttpResponseMessage response = await authClient.PostAsync(new Uri(tokenUrl), content);

        if (response.IsSuccessStatusCode)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var tokenApiResult = await JsonSerializer.DeserializeAsync
                <IdfMobiliteTokenDto>(responseStream);
            if (tokenApiResult == null)
            {
                LoggerService.Instance.Log(
                    logLevel: LogLevel.Error,
                    operationName: $"{nameof(GeoClient)}.{nameof(this.GetTokenAsync)}",
                    message: "Token result is null.");
                throw new ApiException("GetIdfmToken");
            }
            return tokenApiResult;
        }
        else
        {
            LoggerService.Instance.Log(
                logLevel: LogLevel.Error,
                operationName: $"{nameof(GeoClient)}.{nameof(this.GetTokenAsync)}",
                message: "Cannot fetch token from IDFM API.");
            throw new ApiException("GetIdfmToken");
        }
    }

    /// <inheritdoc/>
    public async Task<Journey?> GetTransportationJourneyAsync(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates, string authToken)
    {
        var requestMessage = this.craftQuery(
            arrivalTime: arrivalTime,
            fromGeoCoordinates: fromGeoCoordinates,
            toGeoCoordinates: toGeoCoordinates);

        var journeyApiResult = await AuthentifiedApiCallAsync<IdfMobiliteJourneyDto>(requestMessage, authToken);

        return journeyApiResult?.Journeys.FirstOrDefault()?.ToJourney();
    }

    public async Task<T?> AuthentifiedApiCallAsync<T>(HttpRequestMessage requestMessage, string authToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authToken);
        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync
                <T>(responseStream);
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            LoggerService.Instance.Log(
                logLevel: LogLevel.Warning,
                operationName: $"{nameof(GeoClient)}.{nameof(this.AuthentifiedApiCallAsync)}",
                message: "Token is invalid.");
            return default(T?);
        }
        else
        {
            LoggerService.Instance.Log(
                logLevel: LogLevel.Error,
                operationName: $"{nameof(GeoClient)}.{nameof(this.AuthentifiedApiCallAsync)}",
                message: "Cannot fetch data from IDFM API.");
            return default(T?);
        }
    }

    private HttpRequestMessage craftQuery(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append(journeyQuery);
        urlBuilder.Append(
            $"from={fromGeoCoordinates.Latitude.ToString(CultureInfo.InvariantCulture)}" +
            $";{fromGeoCoordinates.Longitude.ToString(CultureInfo.InvariantCulture)}");
        urlBuilder.Append(
            $"&to={toGeoCoordinates.Latitude.ToString(CultureInfo.InvariantCulture)}" +
            $";{toGeoCoordinates.Longitude.ToString(CultureInfo.InvariantCulture)}");
        urlBuilder.Append($"&datetime={arrivalTime.ToString("yyyyMMddHHmmss")}");
        urlBuilder.Append("&first_section_mode[]=walking");
        urlBuilder.Append("&last_section_mode[]=walking");
        urlBuilder.Append("&forbidden_uris[]=" +
            "physical_mode:Air," +
            "physical_mode:Boat," +
            "physical_mode:Ferry," +
            "physical_mode:Taxi");
        urlBuilder.Append("&count=1");

        return new HttpRequestMessage(method: HttpMethod.Get, requestUri: urlBuilder.ToString());
    }
}
