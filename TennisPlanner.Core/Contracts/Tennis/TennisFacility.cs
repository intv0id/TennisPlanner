using System;
using TennisPlanner.Core.Contracts.Location;

namespace TennisPlanner.Core.Contracts.Tennis
{
    public class TennisFacility
    {
        public string Name;
        public string Url;
        public string Address;
        public int CourtsCount;
        public GeoCoordinates Coordinates;

        public TennisFacility(string name, string url, string address, int courtsCount, GeoCoordinates coordinates)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            CourtsCount = courtsCount;
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
        }
    }
}
