namespace TennisPlanner.Core.Configuration;

/// <inheritdoc/>
public class LocalConfigurationProvider : IAppConfigurationProvider
{
    /// <inheritdoc/>
    public string GetApiBaseUrl() => "http://localhost:7071/api/";
}
