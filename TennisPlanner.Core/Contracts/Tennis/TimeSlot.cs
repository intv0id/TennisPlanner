using System;
using System.Text.Json.Serialization;
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

        [JsonPropertyName("time")]
        public TimeRange TimeRange { get; }
        [JsonPropertyName("status")]
        public CourtStatus Status { get; }
        [JsonPropertyName("court")]
        public TennisCourt CourtInfo { get; }
    }
}
