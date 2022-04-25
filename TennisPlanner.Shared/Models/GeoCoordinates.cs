using System.Text.Json.Serialization;

namespace TennisPlanner.Shared.Models;

public class GeoCoordinates
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
}
