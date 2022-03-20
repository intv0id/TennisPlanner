using TennisPlanner.Core.Enum;

namespace TennisPlanner.Core.Contracts
{
    public class TimeSlot
    {
        public TimeSlot(TimeRange timeRange, CourtStatus status, TennisCourt courtInfo)
        {
            TimeRange = timeRange;
            Status = status;
            CourtInfo = courtInfo;
            TravelInfo = new();
        }

        public TimeRange TimeRange { get; }
        public CourtStatus Status { get; }
        public TennisCourt CourtInfo { get; }
        public TravelInfo TravelInfo { get; }
    }
}
