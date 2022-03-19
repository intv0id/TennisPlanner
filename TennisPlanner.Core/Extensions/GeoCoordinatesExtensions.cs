using System;
using TennisPlanner.Core.Contracts.Location;

namespace TennisPlanner.Core.Extensions
{
    public static class GeoCoordinatesExtensions
    {
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
}
