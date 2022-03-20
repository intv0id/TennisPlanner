using ICSharpCode.Decompiler.Util;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Resources;
using TennisPlanner.Core.Clients;
using TennisPlanner.Core.Resources;

ResourceSet resourceSet =
    Locations.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
var result = new Dictionary<string, string>();
var geoClient = new GeoClient();
using ResXResourceWriter resx = new ResXResourceWriter(@".\GeoLocations.resx");
foreach (DictionaryEntry entry in resourceSet)
{
    string resourceKey = entry.Key.ToString();
    string resource = entry.Value.ToString();

    var latLon = 
        geoClient.GetAddressAutocompleteAsync(partialAddress: resource)
        .GetAwaiter().GetResult()
        .FirstOrDefault();
    
    if (latLon == null)
    {
        Console.WriteLine($"Cannot find address for {resourceKey}.");
    }
    else
    {
        Console.WriteLine($"{resourceKey} found in {latLon.Properties.City}.");
        var (lat, lon) = (latLon.Geometry.Coordinates[0], latLon.Geometry.Coordinates[1]);
        result.Add(resourceKey, $"{lat};{lon}");
        resx.AddResource(resourceKey, $"{lat};{lon}");
    }
}