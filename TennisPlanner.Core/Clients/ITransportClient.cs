using System;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts.Location;
using TennisPlanner.Core.Contracts.Transport;

namespace TennisPlanner.Core.Clients;

/// <summary>
/// A client that fetches journeys information.
/// </summary>
public interface ITransportClient
{
    /// <summary>
    /// Get journey in public transports between
    /// <paramref name="fromGeoCoordinates"/> and <paramref name="toGeoCoordinates"/>, 
    /// to arrive before <paramref name="arrivalTime"/>.
    /// </summary>
    /// <param name="arrivalTime">The estimated arrival time.</param>
    /// <param name="fromGeoCoordinates">The starting point.</param>
    /// <param name="toGeoCoordinates">The final destination.</param>
    /// <returns>A <see cref="JourneyDurationDto"/> object when the search found a result, else null.</returns>
    Task<JourneyDurationDto?> GetTransportationJourneyAsync(
        DateTime arrivalTime, 
        GeoCoordinates fromGeoCoordinates, 
        GeoCoordinates toGeoCoordinates);
}
