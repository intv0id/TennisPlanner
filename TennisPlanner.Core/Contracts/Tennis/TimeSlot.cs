using System;
using TennisPlanner.Core.Contracts.Tennis;
using TennisPlanner.Core.Enum;

namespace TennisPlanner.Core.Contracts
{
    public class TimeSlot
    {
        public TimeSlot(TimeRange timeRange, CourtStatus status, TennisCourt courtInfo)
        {
            TimeRange = timeRange ?? throw new ArgumentNullException(nameof(timeRange));
            Status = status;
            CourtInfo = courtInfo ?? throw new ArgumentNullException(nameof(courtInfo));
        }

        public TimeRange TimeRange { get; }
        public CourtStatus Status { get; }
        public TennisCourt CourtInfo { get; }
    }
}
