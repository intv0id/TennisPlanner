using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Transport
{
    public class JourneyDuration
    {
        [JsonPropertyName("total")]
        public int TotalDurationInSeconds { get; set; }

        [JsonPropertyName("walking")]
        public int WalkingDurationInSeconds { get; set; }
    }
}