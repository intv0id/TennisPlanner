using System;
using TennisPlanner.Core.Contracts.Location;

namespace TennisPlanner.Core.Extensions;

/// <summary>
/// Extensions for <see cref="GeoCoordinates"/>.
/// </summary>
public static class GeoCoordinatesExtensions
{
    /// <summary>
    /// Converts a tuple of <see cref="double"/> to <see cref="GeoCoordinates"/>
    /// </summary>
    /// <param name="coordinates">The input coordinates.</param>
    /// <returns>The output coordinates.</returns>
    /// <exception cref="ArgumentException">Raised if the input array has more or less than two elements.</exception>
    public static GeoCoordinates ToGeoCoordinates(this double[] coordinates)
    {
        if (coordinates.Length != 2)
        {
            throw new ArgumentException($"Cannot convert coordinates of dimension {coordinates.Length}");
        }

        return new GeoCoordinates() 
        { 
            Latitude = coordinates[0], 
            Longitude = coordinates[1] 
        };
    }
}
