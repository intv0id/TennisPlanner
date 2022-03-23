using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace TennisPlanner.Core.Configuration
{
    public class AzureConfigurationProvider : IAppConfigurationProvider
    {
        private const string IdfMobiliteClientIdName = "TennisPlanner.Core.Clients.IdfMobilitesClient.ClientId";
        private const string IdfMobiliteClientSecretName = "TennisPlanner.Core.Clients.IdfMobilitesClient.ClientSecret";
        
        private readonly IConfiguration _configuration;

        public AzureConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string clientId, string clientSecret) GetIdfMobiliteClientCredentials()
        {
            var clientId = _configuration[IdfMobiliteClientIdName];
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ConfigurationErrorsException($"Cannot find {IdfMobiliteClientIdName} in configuration.");
            }

            var clientSecret = _configuration[IdfMobiliteClientSecretName];
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ConfigurationErrorsException($"Cannot find {IdfMobiliteClientSecretName} in configuration.");
            }

            return (clientId, clientSecret);
        }
    }
}
