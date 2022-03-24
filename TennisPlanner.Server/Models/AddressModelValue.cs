using System.Text.Json.Serialization;
using TennisPlanner.Core.Contracts.Location;

namespace TennisPlanner.Server.Models;

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
