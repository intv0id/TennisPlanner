using System.Text.Json.Serialization;

namespace TennisPlanner.Shared.Models;

public class FiltersDto
{
    [JsonPropertyName("hrl")]
    public List<HourRangeSelectorModel> HourRangeList { get; set; }
    [JsonPropertyName("al")]
    public List<AddressModel> AddressesList { get; set; }
}
