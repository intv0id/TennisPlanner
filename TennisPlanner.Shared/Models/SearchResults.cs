using TennisPlanner.Shared.Enums;

namespace TennisPlanner.Shared.Models;

public class SearchResults
{
    public SearchResults(List<SearchResultItem> searchResultItems, List<GeoCoordinates> fromGeoCoordinates)
    {
        SearchStatus = SearchResultsStatus.Ready;
        SearchResultItems = searchResultItems ?? throw new ArgumentNullException(nameof(searchResultItems));
        FromGeoCoordinates = fromGeoCoordinates ?? throw new ArgumentNullException(nameof(fromGeoCoordinates));
    }

    public SearchResults(SearchFailureErrorCode errorCode)
    {
        SearchStatus = SearchResultsStatus.Failed;
        ErrorCode = errorCode;
    }

    public SearchResults()
    {
        SearchStatus = SearchResultsStatus.Loading;
    }

    public SearchResultsStatus SearchStatus { get; }
    public List<SearchResultItem>? SearchResultItems { get; }
    public List<GeoCoordinates>? FromGeoCoordinates { get; }
    public SearchFailureErrorCode? ErrorCode { get; }
}
