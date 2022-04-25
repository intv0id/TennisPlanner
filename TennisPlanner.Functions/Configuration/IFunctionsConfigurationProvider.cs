namespace TennisPlanner.Functions.Configuration;

/// <summary>
/// The configuration provider service.
/// </summary>
public interface IFunctionsConfigurationProvider
{
    /// <summary>
    /// Returns the credentials for fetching token to Ile de France mobilités API.
    /// </summary>
    /// <returns>The client id and secret stored in the config.</returns>
    (string clientId, string clientSecret) GetIdfMobiliteClientCredentials();
}
