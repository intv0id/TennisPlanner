using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts;

namespace TennisPlanner.Core.Clients
{
    public interface ITennisClient
    {
        public Task<List<TennisCourt>> GetTennisCourtsListAsync();

        public Task<List<TimeSlot>> GetTimeSlotListAsync(TennisCourt tennisCourt, DateTime day);
    }
}