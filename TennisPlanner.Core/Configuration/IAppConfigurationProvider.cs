namespace TennisPlanner.Core.Configuration;

/// <summary>
/// The configuration provider service.
/// </summary>
public interface IAppConfigurationProvider
{
    /// <summary>
    /// Returns the application api base url.
    /// </summary>
    /// <returns>The application api base url.</returns>
    string GetApiBaseUrl();
}
