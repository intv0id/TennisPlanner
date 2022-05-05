using System.Text.Json.Serialization;

namespace TennisPlanner.Shared.Models;

public class FiltersProfileDto
{
    public FiltersProfileDto(List<HourRangeSelectorModel> hourRangeList, List<AddressModel> addressesList, string profileName, string? id = null)
    {
        HourRangeList = hourRangeList ?? throw new ArgumentNullException(nameof(hourRangeList));
        AddressesList = addressesList ?? throw new ArgumentNullException(nameof(addressesList));
        ProfileName = profileName ?? throw new ArgumentNullException(nameof(profileName));
        Id = id;
    }

    [JsonPropertyName("hrl")]
    public List<HourRangeSelectorModel> HourRangeList { get; set; }
    [JsonPropertyName("al")]
    public List<AddressModel> AddressesList { get; set; }
    [JsonPropertyName("pn")]
    public string ProfileName { get; set; }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
