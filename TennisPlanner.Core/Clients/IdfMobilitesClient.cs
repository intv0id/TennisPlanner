using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using TennisPlanner.Shared.Services.Logging;
using Timer = System.Timers.Timer;

namespace TennisPlanner.Core.Clients
{
    public class IdfMobilitesClient : ITransportClient
    {
        const string tokenUrl = "https://as.api.iledefrance-mobilites.fr/api/oauth/token";
        const string baseApiUrl = "https://traffic.api.iledefrance-mobilites.fr/v2/mri/coverage/idfm/";
        const string journeyQuery = "journeys?";
        const int refreshTokenTimeBuffer = 60;

        private readonly HttpClient _httpClient;
        private readonly IAppConfigurationProvider _configurationProvider;
        private readonly SemaphoreSlim refreshingTokenSemaphore = new SemaphoreSlim(initialCount: 1);
        private readonly Timer? tokenRefreshTimer = new Timer();

        public IdfMobilitesClient(IAppConfigurationProvider configurationProvider)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseApiUrl),
            };

            _configurationProvider = configurationProvider;

            tokenRefreshTimer.Elapsed += new ElapsedEventHandler(RefreshAccessTokenEvent);
            _ = refreshAccessToken();
        }

        public async Task<JourneyDuration?> GetTransportationTimeInMinutesAsync(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates)
        {
            var requestMessage = this.craftQuery(
                arrivalTime: arrivalTime,
                fromGeoCoordinates: fromGeoCoordinates,
                toGeoCoordinates: toGeoCoordinates);

            var journeyApiResult = await AuthentifiedApiCallAsync<IdfMobiliteJourneyResponse>(requestMessage);

            return journeyApiResult?.Journeys.FirstOrDefault()?.JourneyDuration;
        }

        private async Task refreshAccessToken()
        {
            await refreshingTokenSemaphore.WaitAsync();
            try
            {
                var (clientId, clientSecret) = _configurationProvider.GetIdfMobiliteClientCredentials();

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
                        <IdfMobiliteTokenResponse>(responseStream);
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", tokenApiResult.AccessToken);
                    
                    tokenRefreshTimer.Interval = (tokenApiResult.ExpirationDuration - refreshTokenTimeBuffer)*1000;
                    tokenRefreshTimer.Start();
                }
                else
                {
                    LoggerService.Instance.Log(
                        logLevel: LogLevel.Error,
                        operationName: $"{nameof(GeoClient)}.{nameof(this.GetTransportationTimeInMinutesAsync)}",
                        message: "Cannot fetch data from IDFM API.");
                }
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Log(
                       logLevel: LogLevel.Error,
                       operationName: $"{nameof(GeoClient)}.{nameof(this.GetTransportationTimeInMinutesAsync)}",
                       message: "Exception thrown while fetching token from IDFM API.",
                       exception: ex);
                throw;
            }
            finally
            {
                refreshingTokenSemaphore.Release();
            }
            
        }

        private void RefreshAccessTokenEvent(object? sender, ElapsedEventArgs e)
        {
            tokenRefreshTimer.Stop();
            _ = refreshAccessToken();
        }

        public async Task<T?> AuthentifiedApiCallAsync<T>(HttpRequestMessage requestMessage, bool isRetry = false)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync
                    <T>(responseStream);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized && !isRetry)
            {
                LoggerService.Instance.Log(
                    logLevel: LogLevel.Warning,
                    operationName: $"{nameof(GeoClient)}.{nameof(this.GetTransportationTimeInMinutesAsync)}",
                    message: "Token is invalid.");
                await refreshAccessToken();
                return await AuthentifiedApiCallAsync<T>(
                    requestMessage: requestMessage,
                    isRetry = true);
            }
            else
            {
                LoggerService.Instance.Log(
                    logLevel: LogLevel.Error,
                    operationName: $"{nameof(GeoClient)}.{nameof(this.GetTransportationTimeInMinutesAsync)}",
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
}
