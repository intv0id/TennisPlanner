namespace TennisPlanner.Core.Configuration
{
    public interface IAppConfigurationProvider
    {
        (string clientId, string clientSecret) GetIdfMobiliteClientCredentials();
    }
}
