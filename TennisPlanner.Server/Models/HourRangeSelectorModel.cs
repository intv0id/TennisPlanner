using System.Text.Json.Serialization;

namespace TennisPlanner.Server.Models;

public class HourRangeSelectorModel

{
    [JsonPropertyName("hr")]
    public IEnumerable<int> HourRange { get; set; }
}
