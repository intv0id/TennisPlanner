using System.Configuration;

namespace TennisPlanner.Core.Clients
{
    public class TokenProvider : ITokenProvider
    {
        private const string IdfMobiliteTokenName = "TennisPlanner.Core.Clients.IdfMobilitesClient.ApiKey";
        public string GetIdfMobiliteToken()
        {
            var apiKey = ConfigurationManager.AppSettings[IdfMobiliteTokenName];
            return apiKey ?? throw new ConfigurationErrorsException($"Cannot find {IdfMobiliteTokenName}.");
        }
    }
}
