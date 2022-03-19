using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Location
{
    /// <summary>
    /// Implementation of the spec defined in <see href="https://github.com/geocoders/geocodejson-spec/tree/master/draft"/>
    /// </summary>
    public class Address
    {
        [JsonPropertyName("geometry")]
        public Geometry Geometry {get; set;}

        [JsonPropertyName("properties")]
        public GeoProperties Properties { get; set; }
    }
}
