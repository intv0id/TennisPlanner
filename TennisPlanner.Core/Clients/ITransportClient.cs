using System;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts.Location;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.Core.Clients;

/// <summary>
/// A client that fetches journeys information.
/// </summary>
public interface ITransportClient
{
    /// <summary>
    /// Gets the connection token.
    /// </summary>
    /// <param name="clientId">The client id.</param>
    /// <param name="clientSecret">The client secret.</param>
    /// <returns>The connection token.</returns>
    Task<IdfMobiliteTokenDto> GetTokenAsync(
        string clientId, 
        string clientSecret);

    /// <summary>
    /// Get journey in public transports between
    /// <paramref name="fromGeoCoordinates"/> and <paramref name="toGeoCoordinates"/>, 
    /// to arrive before <paramref name="arrivalTime"/>.
    /// </summary>
    /// <param name="arrivalTime">The estimated arrival time.</param>
    /// <param name="fromGeoCoordinates">The starting point.</param>
    /// <param name="toGeoCoordinates">The final destination.</param>
    /// <param name="getTokenMethod">A method to get a connection token.</param>
    /// <returns>A <see cref="Journey"/> object when the search found a result, else null.</returns>
    Task<Journey?> GetTransportationJourneyAsync(
        DateTime arrivalTime, 
        GeoCoordinates fromGeoCoordinates, 
        GeoCoordinates toGeoCoordinates,
        Func<IdfMobiliteTokenDto> getTokenMethod);
}
