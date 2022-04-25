namespace TennisPlanner.Shared.Models;

public class SearchResultItem
{
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public GeoCoordinates CourtGeoCoordinates { get; set; }
    public string TennisFacilityName { get; set; }
    public string CourtName { get; set; }
    public string CourtLighting { get; set; }
    public string CourtGround { get; set; }
    public string CourtRoof { get; set; }
    public readonly Dictionary<GeoCoordinates, Task<Journey>> journeyDurationsTasks;

    public SearchResultItem(
        DateTime fromDateTime,
        DateTime toDateTime,
        GeoCoordinates courtGeoCoordinates,
        string tennisFacilityName,
        string courtName,
        string courtLighting,
        string courtGround,
        string courtRoof)
    {
        FromDateTime = fromDateTime;
        ToDateTime = toDateTime;
        CourtGeoCoordinates = courtGeoCoordinates;
        TennisFacilityName = tennisFacilityName;
        CourtName = courtName;
        CourtLighting = courtLighting;
        CourtGround = courtGround;
        CourtRoof = courtRoof;

        journeyDurationsTasks = new Dictionary<GeoCoordinates, Task<Journey>>();
    }
}
