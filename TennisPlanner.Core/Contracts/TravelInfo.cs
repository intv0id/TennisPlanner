using System;
using TennisPlanner.Core.Contracts.Transport;

namespace TennisPlanner.Core.Contracts
{
    public class TravelInfo
    {
        public JourneyDuration? JourneyDurationFromAdress1 { get; set; }
        public JourneyDuration? JourneyDurationFromAdress2 { get; set; }
    }
}
