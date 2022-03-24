using System.Text.Json.Serialization;
using TennisPlanner.Core.Contracts.Location;

namespace TennisPlanner.Server.Models;

internal class FiltersDto
{
    [JsonPropertyName("hrl")]
    public List<HourRangeSelectorModel> HourRangeList { get; set; }
    [JsonPropertyName("al")]
    public List<AddressModel> AddressesList { get; set; }
}
