using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

public class ResultsChangedArgs
{
    public ResultsChangedArgs(SearchResults newSearchResults)
    {
        NewSearchResults = newSearchResults ?? throw new ArgumentNullException(nameof(newSearchResults));
    }

    public SearchResults NewSearchResults { get; }
}
