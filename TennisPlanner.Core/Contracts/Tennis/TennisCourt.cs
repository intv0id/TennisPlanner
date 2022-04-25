using System;
using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Tennis
{
    public class TennisCourt
    {
        public TennisCourt(TennisFacility facility, string title, string roof, string ground, string light)
        {
            Facility = facility ?? throw new ArgumentNullException(nameof(facility));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Roof = roof ?? throw new ArgumentNullException(nameof(roof));
            Ground = ground ?? throw new ArgumentNullException(nameof(ground));
            Light = light ?? throw new ArgumentNullException(nameof(light));
        }

        [JsonPropertyName("facility")]
        public TennisFacility Facility { get; }
        [JsonPropertyName("title")]
        public string Title { get; }
        [JsonPropertyName("roof")]
        public string Roof { get; }
        [JsonPropertyName("ground")]
        public string Ground { get; }
        [JsonPropertyName("light")]
        public string Light { get; }
    }
}
