using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Transport
{
    public class JourneyDto
    {
        [JsonPropertyName("durations")]
        public JourneyDurationDto JourneyDuration { get; set; }
    }
}
