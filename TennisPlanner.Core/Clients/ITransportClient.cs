namespace TennisPlanner.Core.Clients
{
    using System;
    using TennisPlanner.Core.Contracts.Location;

    public interface ITransportClient
    {
        DateTime GetTransportationTime(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates);
    }
}
