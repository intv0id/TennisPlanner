using System;
using TennisPlanner.Core.Contracts.Location;

namespace TennisPlanner.Core.Clients
{
    public class RatpClient : ITransportClient
    {
        public DateTime GetTransportationTime(DateTime arrivalTime, GeoCoordinates fromAdress, GeoCoordinates toAdress)
        {
            throw new NotImplementedException();
        }
    }
}
