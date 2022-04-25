using TennisPlanner.Core.Contracts;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

public interface ITennisPlannerAPIService
{
    Task<IEnumerable<TimeSlot>> GetTennisDataAsync(DateTime day);
    Task<Journey?> GetTransportationJourneyAsync(DateTime arrivalTime, GeoCoordinates fromGeoCoordinates, GeoCoordinates toGeoCoordinates);
}
