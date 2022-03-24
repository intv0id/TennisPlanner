using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Location
{
    public class GeoApiDto
    {
        [JsonPropertyName("features")]
        public IEnumerable<AddressDto> Addresses { get; set; }
    }
}
