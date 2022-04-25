using System.Text.Json.Serialization;

namespace TennisPlanner.Shared.Models
{
    public class Journey
    {
        public int TotalDurationInSeconds { get; set; }

        public int WalkingDurationInSeconds { get; set; }
    }
}