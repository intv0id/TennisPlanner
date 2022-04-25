using System;
using System.Text.Json.Serialization;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.Core.Contracts.Tennis;

public class TennisFacility
{
    public TennisFacility(string name, string url, string address, int courtsCount, GeoCoordinates coordinates)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Url = url ?? throw new ArgumentNullException(nameof(url));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        CourtsCount = courtsCount;
        Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
    }

    [JsonPropertyName("name")]
    public string Name { get; }
    [JsonPropertyName("url")]
    public string Url { get; }
    [JsonPropertyName("address")]
    public string Address { get; }
    [JsonPropertyName("courts_count")]
    public int CourtsCount { get; }
    [JsonPropertyName("coordinates")]
    public GeoCoordinates Coordinates { get; }
}
