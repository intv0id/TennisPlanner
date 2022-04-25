using System;
using System.Configuration;

namespace TennisPlanner.Functions.Configuration;

/// <inheritdoc/>
public class FunctionsConfigurationProvider : IFunctionsConfigurationProvider
{
    private const string IdfMobiliteClientIdName = "IdfMobilitesClient_ClientId";
    private const string IdfMobiliteClientSecretName = "IdfMobilitesClient_ClientSecret";

    /// <inheritdoc/>
    public (string clientId, string clientSecret) GetIdfMobiliteClientCredentials()
    {
        var clientId = GetConfigValue(IdfMobiliteClientIdName);
        if (string.IsNullOrEmpty(clientId))
        {
            throw new ConfigurationErrorsException($"Cannot find {IdfMobiliteClientIdName} in configuration.");
        }

        var clientSecret = GetConfigValue(IdfMobiliteClientSecretName);
        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new ConfigurationErrorsException($"Cannot find {IdfMobiliteClientSecretName} in configuration.");
        }

        return (clientId, clientSecret);
    }

    private static string GetConfigValue(string name)
    {
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }
}
