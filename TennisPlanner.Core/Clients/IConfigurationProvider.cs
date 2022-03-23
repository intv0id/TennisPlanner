namespace TennisPlanner.Core.Clients
{
    public interface IConfigurationProvider
    {
        (string clientId, string clientSecret) GetIdfMobiliteClientCredentials();
    }
}
