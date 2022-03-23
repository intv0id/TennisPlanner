using System.Configuration;

namespace TennisPlanner.Core.Configuration
{
    public class LocalConfigurationProvider : IAppConfigurationProvider
    {
        private const string IdfMobiliteClientIdName = "TennisPlanner.Core.Clients.IdfMobilitesClient.ClientId";
        private const string IdfMobiliteClientSecretName = "TennisPlanner.Core.Clients.IdfMobilitesClient.ClientSecret";

        public (string clientId, string clientSecret) GetIdfMobiliteClientCredentials()
        {
            var clientId = ConfigurationManager.AppSettings[IdfMobiliteClientIdName];
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ConfigurationErrorsException($"Cannot find {IdfMobiliteClientIdName} in configuration.");
            }

            var clientSecret = ConfigurationManager.AppSettings[IdfMobiliteClientSecretName];
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ConfigurationErrorsException($"Cannot find {IdfMobiliteClientSecretName} in configuration.");
            }

            return (clientId, clientSecret);
        }
    }
}
