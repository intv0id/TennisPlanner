namespace TennisPlanner.Core.Clients
{
    using System;
    using System.Threading.Tasks;
    using TennisPlanner.Core.Contracts.Location;
    using TennisPlanner.Core.Contracts.Transport;

    public interface ITransportClient
    {
        Task<JourneyDuration?> GetTransportationTimeInMinutesAsync(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates);
    }
}
