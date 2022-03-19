using System.Collections.Generic;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts.Location;

namespace TennisPlanner.Core.Clients
{
    public interface IGeoClient
    {
        Task<IEnumerable<Address>> GetAddressAutocompleteAsync(string partialAddress);
    }
}
