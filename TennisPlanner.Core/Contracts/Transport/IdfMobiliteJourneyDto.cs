using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Transport
{
    public class IdfMobiliteJourneyDto
    {
        [JsonPropertyName("journeys")]
        public JourneyDto[] Journeys { get; set; } 
    }
}
