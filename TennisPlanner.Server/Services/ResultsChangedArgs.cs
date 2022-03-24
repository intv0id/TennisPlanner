using TennisPlanner.Server.Models;

namespace TennisPlanner.Server.Services
{
    public class ResultsChangedArgs
    {
        public ResultsChangedArgs(SearchResults newSearchResults)
        {
            NewSearchResults = newSearchResults ?? throw new ArgumentNullException(nameof(newSearchResults));
        }

        public SearchResults NewSearchResults { get; }
    }
}