using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Location
{
    /// <summary>
    /// Implementation of the spec defined in <see href="https://github.com/geocoders/geocodejson-spec/tree/master/draft"/>
    /// </summary>
    public class AddressDto
    {
        [JsonPropertyName("geometry")]
        public GeometryDto Geometry {get; set;}

        [JsonPropertyName("properties")]
        public GeoPropertiesDto Properties { get; set; }

        [JsonIgnore]
        public bool Validated => Geometry != null && Properties != null;
    }
}
