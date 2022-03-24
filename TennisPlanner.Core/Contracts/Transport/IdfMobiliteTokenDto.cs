using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Transport
{
    public class IdfMobiliteTokenDto
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpirationDuration { get; set; }
        [JsonPropertyName("scope")]
        public string TokenScope { get; set; }
    }
}
