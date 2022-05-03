using TennisPlanner.Core.Contracts;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

public interface ITennisPlannerAPIService
{
    Task<IEnumerable<TimeSlot>> GetTennisDataAsync(DateTime day);
    Task<IdfMobiliteTokenDto> GetTokenAsync();
}
