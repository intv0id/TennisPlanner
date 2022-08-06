namespace TennisPlanner.Functions.Configuration;

/// <summary>
/// The configuration provider service.
/// </summary>
public interface IFunctionsConfigurationProvider
{
    /// <summary>
    /// Returns the api token for the PRIM API.
    /// </summary>
    /// <returns>The api token stored in the config.</returns>
    string GetPrimToken();
}
