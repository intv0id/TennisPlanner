using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

/// <summary>
/// The search engine service for tennis availabilities and journey computations.
/// </summary>
public interface ISearchResultDataProvider
{
    /// <summary>
    /// Init the data provider and start search with the provided parameters.
    /// </summary>
    /// <param name="dateTime">The date on which to search for availabilities.</param>
    Task Init(DateTime dateTime);

    /// <summary>
    /// Gets the computed search results.
    /// </summary>
    /// <returns>An instance of <see cref="SearchResults"/></returns>
    SearchResults GetSearchResults();

    /// <summary>
    /// Add a player on which to compute the journeys. Starts the computation tasks.
    /// </summary>
    /// <param name="address">The addess of the player.</param>
    void AddPlayerLocation(AddressModel address);

    /// <summary>
    /// Removes a player on which to compute the journeys. Ends the computation tasks if not finished.
    /// </summary>
    /// <param name="address">The address of the player.</param>
    void RemovePlayerLocation(AddressModel address);

    /// <summary>
    /// Returns a filtered list of results items based on the filters in the filter service.
    /// </summary>
    /// <param name="resultItems">The result list to filter.</param>
    /// <returns>The filtered list.</returns>
    IEnumerable<SearchResultItem> Filter(IEnumerable<SearchResultItem> resultItems);

    /// <summary>
    /// Event triggered when changes occur in the results.
    /// </summary>
    event EventHandler<ResultsChangedArgs> ResultsChanged;
}
