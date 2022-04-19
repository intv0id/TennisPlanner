using System;

namespace TennisPlanner.Core.Contracts
{
    public class TimeRange
    {
        public TimeRange(DateTime startHour, DateTime endHour)
        {
            StartHour = startHour;
            EndHour = endHour;
        }

        public DateTime StartHour { get; }
        public DateTime EndHour { get; }
    }
}
