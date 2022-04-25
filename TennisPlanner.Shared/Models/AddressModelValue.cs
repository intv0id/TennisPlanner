using System.Text.Json.Serialization;

namespace TennisPlanner.Shared.Models;

public class AddressModelValue
{
    public AddressModelValue(string displayName, GeoCoordinates geoCoordinates)
    {
        DisplayName = displayName;
        GeoCoordinates = geoCoordinates;
    }

    [JsonPropertyName("dn")]
    public string DisplayName { get; }
    [JsonPropertyName("gc")]
    public GeoCoordinates GeoCoordinates { get; }
}
