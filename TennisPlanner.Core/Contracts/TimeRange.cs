using System;

namespace TennisPlanner.Core.Contracts
{
    public class TimeRange
    {
        public TimeRange(int startHour, int endHour)
        {
            StartHour = startHour;
            EndHour = endHour;
        }

        public int StartHour { get; }
        public int EndHour { get; }
    }
}
