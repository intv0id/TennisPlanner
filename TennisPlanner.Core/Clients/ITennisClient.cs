using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts;

namespace TennisPlanner.Core.Clients
{
    public interface ITennisClient
    {
        public Task<IEnumerable<TennisFacility>> GetTennisCourtsListAsync();

        public Task<IEnumerable<TimeSlot>> GetTimeSlotListAsync(TennisFacility tennisCourt, DateTime day);
    }
}