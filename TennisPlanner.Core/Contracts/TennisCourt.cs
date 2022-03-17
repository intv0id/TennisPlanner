namespace TennisPlanner.Core.Contracts
{
    public class TennisCourt
    {
        public TennisCourt(TennisFacility facility, string title, object roof, object ground, object light)
        {
            Facility = facility;
            Title = title;
            Roof = roof;
            Ground = ground;
            Light = light;
        }

        public TennisFacility Facility { get; }
        public string Title { get; }
        public object Roof { get; }
        public object Ground { get; }
        public object Light { get; }
    }
}
