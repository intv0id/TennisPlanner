﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts.Location;
using TennisPlanner.Shared.Services.Logging;

namespace TennisPlanner.Core.Clients
{
    /// <inheritdoc/>
    public class GeoClient : IGeoClient
    {
        private const string ApiBaseUrl = "https://api-adresse.data.gouv.fr/search/";

        private readonly HttpClient _httpClient;

        public GeoClient()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(ApiBaseUrl),
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Address>> GetAddressAutocompleteAsync(string partialAddress)
        {
            var request = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: $"?q={partialAddress}");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
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