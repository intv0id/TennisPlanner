using System;
using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts
{
    public class TimeRange
    {
        public TimeRange(DateTime startHour, DateTime endHour)
        {
            StartHour = startHour;
            EndHour = endHour;
        }

        [JsonPropertyName("start")]
        public DateTime StartHour { get; }
        [JsonPropertyName("end")]
        public DateTime EndHour { get; }
    }
}
