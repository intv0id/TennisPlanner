using System.Text.Json.Serialization;

namespace TennisPlanner.Shared.Models;

public class JourneyRequestDto
{
    [JsonPropertyName("arrivalTime")]
    public DateTime ArrivalDateTime { get; set; }
    
    [JsonPropertyName("FromGeo")]
    public GeoCoordinates FromGeoCoordinates { get; set; }

    [JsonPropertyName("ToGeo")]
    public GeoCoordinates ToGeoCoordinates { get; set; }

}
