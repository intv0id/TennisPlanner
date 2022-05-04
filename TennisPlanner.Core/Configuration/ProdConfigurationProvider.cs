using System;

namespace TennisPlanner.Core.Configuration;

/// <inheritdoc/>
public class ProdConfigurationProvider : IAppConfigurationProvider
{
    private readonly ProductionEnvironment _environment;

    public ProdConfigurationProvider(ProductionEnvironment environment)
    {
        _environment = environment;
    }

    /// <inheritdoc/>
    public string GetApiBaseUrl() => (_environment) switch
    {
        ProductionEnvironment.Prod => "https://purple-water-086d66603.1.azurestaticapps.net/api/",
        ProductionEnvironment.Canary => "https://happy-desert-078890e03.1.azurestaticapps.net/api/",
        _ => throw new ArgumentNullException(),
    };
}
