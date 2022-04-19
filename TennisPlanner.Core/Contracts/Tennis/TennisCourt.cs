using System;

namespace TennisPlanner.Core.Contracts.Tennis
{
    public class TennisCourt
    {
        public TennisCourt(TennisFacility facility, string title, string roof, string ground, string light)
        {
            Facility = facility ?? throw new ArgumentNullException(nameof(facility));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Roof = roof ?? throw new ArgumentNullException(nameof(roof));
            Ground = ground ?? throw new ArgumentNullException(nameof(ground));
            Light = light ?? throw new ArgumentNullException(nameof(light));
        }

        public TennisFacility Facility { get; }
        public string Title { get; }
        public string Roof { get; }
        public string Ground { get; }
        public string Light { get; }
    }
}
