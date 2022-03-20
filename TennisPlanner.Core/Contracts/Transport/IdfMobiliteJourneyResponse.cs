using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Transport
{
    public class IdfMobiliteJourneyResponse
    {
        [JsonPropertyName("journeys")]
        public Journey[] Journeys { get; set; } 
    }
}
