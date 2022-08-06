using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace TennisPlanner.Shared.Models;

public class GeoCoordinates
{
    /// <summary>
    /// Gets or sets the latitude.
    /// </summary>
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude.
    /// </summary>
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(this.Latitude.ToString(CultureInfo.InvariantCulture));
        sb.Append(";");
        sb.Append(this.Longitude.ToString(CultureInfo.InvariantCulture));
        return sb.ToString();
    }
}
