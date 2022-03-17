using TennisPlanner.Core.Enum;

namespace TennisPlanner.Core.Contracts
{
    public class TimeSlot
    {
        public TimeSlot(string time, CourtStatus status, TennisCourt courtInfo)
        {
            Time = time;
            Status = status;
            CourtInfo = courtInfo;
        }

        public string Time { get; }
        public CourtStatus Status { get; }
        public TennisCourt CourtInfo { get; }
    }
}
