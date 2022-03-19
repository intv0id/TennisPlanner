using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts.Location;
using TennisPlanner.Shared.Services.Logging;

namespace TennisPlanner.Core.Clients
{
    public class GeoClient : IGeoClient
    {
        private const string ApiBaseUrl = "https://api-adresse.data.gouv.fr/search/?q=";

        private readonly HttpClient _httpClient;

        public GeoClient(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<IEnumerable<Address>> GetAddressAutocompleteAsync(string partialAddress)
        {
            // TODO: escape html char
            var request = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: $"{ApiBaseUrl} + {partialAddress}");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var a = await response.Content.ReadAsStringAsync();

                var geoApiResult = await JsonSerializer.DeserializeAsync
                    <GeoAPIContract>(responseStream);

                return geoApiResult?.Addresses ?? new List<Address>();
            }
            else
            {
                LoggerService.Instance.Log(
                    logLevel: LogLevel.Error, 
                    operationName: $"{nameof(GeoClient)}.{nameof(this.GetAddressAutocompleteAsync)}", 
                    message: "Cannot fetch data from geo API.");
                return new List<Address>();
            }
        }
    }
}
