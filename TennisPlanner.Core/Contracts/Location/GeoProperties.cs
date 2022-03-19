using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Location
{
    public class GeoProperties
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("score")]
        public double Score { get; set; }
        [JsonPropertyName("housenumber")]
        public string HouseNumber { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("postcode")]
        public string PostCode { get; set; }
        [JsonPropertyName("x")]
        public double X { get; set; }
        [JsonPropertyName("y")]
        public double Y { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("context")]
        public string Context { get; set; }
        [JsonPropertyName("importance")]
        public double Importance { get; set; }
        [JsonPropertyName("street")]
        public string Street { get; set; }
    }
}