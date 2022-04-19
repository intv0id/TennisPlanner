using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts;
using TennisPlanner.Core.Contracts.Tennis;

namespace TennisPlanner.Core.Clients;

/// <summary>
/// A client that fetches information on tennis courts and facilities.
/// </summary>
public interface ITennisClient
{
    /// <summary>
    /// Fetch the list of the available tennis facilities.
    /// </summary>
    /// <returns>An enumerable of <see cref="TennisFacility"/>.</returns>
    public Task<IEnumerable<TennisFacility>> GetTennisFacilitiesListAsync();

    /// <summary>
    /// Fetch the list of available time slots in the given facility for he given day.
    /// </summary>
    /// <param name="tennisCourt">The tennis facility.</param>
    /// <param name="day">The day on when to search for availabilities.</param>
    /// <returns>An enumerable of <see cref="TimeSlot"/>.</returns>
    public Task<IEnumerable<TimeSlot>> GetTimeSlotListAsync(TennisFacility tennisCourt, DateTime day);
}
