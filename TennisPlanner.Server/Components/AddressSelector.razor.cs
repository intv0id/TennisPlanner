using Microsoft.AspNetCore.Components;
using TennisPlanner.Core.Clients;
using TennisPlanner.Core.Extensions;
using TennisPlanner.Server.Models;

namespace TennisPlanner.Server.Components;

public partial class AddressSelector
{
    [Inject]
    IGeoClient GeoClient { get; set; }

    private AddressModelValue? selectedAddress
    {
        get => SelectedAddress;
        set
        {
            SelectedAddress = value;
            SelectedAddressChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public AddressModelValue? SelectedAddress { get; set; }

    [Parameter]
    public EventCallback<AddressModelValue?> SelectedAddressChanged { get; set; }

    public async Task<IEnumerable<AddressModelValue>> GetAddressSuggestionAsync(string text)
    {
        var suggestions = await GeoClient.GetAddressAutocompleteAsync(text);
        return suggestions.Select(s => new AddressModelValue(
            displayName: 
                $"{s.Properties.Name}, " 
                + $"{s.Properties.PostCode}, " 
                + $"{s.Properties.City}", 
            geoCoordinates: s.Geometry.Coordinates.ToGeoCoordinates()));
    }
}
