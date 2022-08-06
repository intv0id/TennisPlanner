using System;
using System.Configuration;

namespace TennisPlanner.Functions.Configuration;

/// <inheritdoc/>
public class FunctionsConfigurationProvider : IFunctionsConfigurationProvider
{
    private const string primApiTokenName = "PrimClient_ApiToken";

    /// <inheritdoc/>
    public string GetPrimToken()
    {
        var apiToken = GetConfigValue(primApiTokenName);
        if (string.IsNullOrEmpty(apiToken))
        {
            throw new ConfigurationErrorsException($"Cannot find {primApiTokenName} in configuration.");
        }

        return apiToken;
    }

    private static string GetConfigValue(string name)
    {
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }
}
