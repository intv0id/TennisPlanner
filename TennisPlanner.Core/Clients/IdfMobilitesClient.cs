using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts.Location;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Services.Logging;

namespace TennisPlanner.Core.Clients
{
    public class IdfMobilitesClient : ITransportClient
    {
        const string baseApiUrl = "https://traffic.api.iledefrance-mobilites.fr/v2/mri/coverage/idfm/journeys?";
        private readonly HttpClient _httpClient;

        public IdfMobilitesClient(ITokenProvider tokenProvider)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenProvider.GetIdfMobiliteToken());
        }

        public async Task<JourneyDuration?> GetTransportationTimeInMinutesAsync(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates)
        {
            var requestMessage = this.craftQuery(
                arrivalTime: arrivalTime,
                fromGeoCoordinates: fromGeoCoordinates,
                toGeoCoordinates: toGeoCoordinates);
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var journeyApiResult = await JsonSerializer.DeserializeAsync
                    <IdfMobiliteJourneyResponse>(responseStream);
                return journeyApiResult?.Journeys.FirstOrDefault()?.JourneyDuration;
            }
            else
            {
                LoggerService.Instance.Log(
                    logLevel: LogLevel.Error,
                    operationName: $"{nameof(GeoClient)}.{nameof(this.GetTransportationTimeInMinutesAsync)}",
                    message: "Cannot fetch data from IDFM API.");
                return null;
            }
        }

        private HttpRequestMessage craftQuery(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(baseApiUrl);
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
