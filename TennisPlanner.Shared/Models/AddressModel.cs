using System.Text.Json.Serialization;

namespace TennisPlanner.Shared.Models;

public class AddressModel
{
    [JsonPropertyName("v")]
    public AddressModelValue? Value { get; set; }

    [JsonIgnore]
    public bool Validated => Value != null;
}
