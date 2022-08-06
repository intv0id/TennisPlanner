using System;
using System.Text.Json.Serialization;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.Core.Contracts.Transport
{
    public class TransportationJourneyRequestDto
    {
        [JsonPropertyName("ArrivalTime")]
        public DateTime ArrivalTime { get; set; }

        [JsonPropertyName("fromGeoCoordinates")]
        public GeoCoordinates FromGeoCoordinates { get; set; }

        [JsonPropertyName("toGeoCoordinates")]
        public GeoCoordinates ToGeoCoordinates { get; set; }
    }
}
