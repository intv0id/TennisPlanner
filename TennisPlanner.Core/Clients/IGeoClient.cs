using System.Collections.Generic;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts.Location;

namespace TennisPlanner.Core.Clients
{
    /// <summary>
    /// The interface for a geo finding API.
    /// </summary>
    public interface IGeoClient
    {
        /// <summary>
        /// Get a list of the most likely addresses matching the given partial address.
        /// </summary>
        /// <param name="partialAddress">The partial address to complete.</param>
        /// <returns>A list of addresses.</returns>
        Task<IEnumerable<Address>> GetAddressAutocompleteAsync(string partialAddress);
    }
}
