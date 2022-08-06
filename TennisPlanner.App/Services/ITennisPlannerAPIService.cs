using TennisPlanner.Core.Contracts;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

/// <summary>
/// API proxy class.
/// </summary>
public interface ITennisPlannerAPIService
{
    /// <summary>
    /// Gets tennis data for a given day.
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    public Task<IEnumerable<TimeSlot>> GetTennisDataAsync(DateTime day);

    /// <summary>
    /// Gets transportation journey 
    /// from <paramref name="fromGeoCoordinates"/> 
    /// to <paramref name="toGeoCoordinates"/>
    /// arriving at <paramref name="arrivalTime"/>.
    /// </summary>
    /// <param name="arrivalTime">The journey arrival time.</param>
    /// <param name="fromGeoCoordinates">The starting point geo coordinates.</param>
    /// <param name="toGeoCoordinates">The arrival point geo coordinates.</param>
    /// <returns>The best journey in duration if this exists. Else null.</returns>
    public Task<Journey?> GetTransportationJourneyAsync(
        DateTime arrivalTime,
        GeoCoordinates fromGeoCoordinates,
        GeoCoordinates toGeoCoordinates);
}
