using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Location
{
    public class GeometryDto
    {
        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; }

        [JsonPropertyName("type")]
        public string GeometryType { get; set; }
    }
}