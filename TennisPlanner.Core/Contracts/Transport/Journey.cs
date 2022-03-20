using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Transport
{
    public class Journey
    {
        [JsonPropertyName("durations")]
        public JourneyDuration JourneyDuration { get; set; }
    }
}
