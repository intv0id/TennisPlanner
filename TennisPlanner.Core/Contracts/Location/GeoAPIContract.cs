using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Location
{
    public class GeoAPIContract
    {
        [JsonPropertyName("features")]
        public IEnumerable<Address> Addresses { get; set; }
    }
}
