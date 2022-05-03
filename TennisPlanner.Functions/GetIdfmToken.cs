using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TennisPlanner.Functions.Configuration;
using TennisPlanner.Core.Clients;
using System.Web.Http;

namespace TennisPlanner.Functions
{
    public static class GetIdfmToken
    {
        [FunctionName("GetIdfmToken")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var configurationProvider = new FunctionsConfigurationProvider();
            var (clientId, clientSecret) = configurationProvider.GetIdfMobiliteClientCredentials();
            var transportClient = new IdfMobilitesClient();

            try
            {
                var value = await transportClient.GetTokenAsync(clientId: clientId, clientSecret: clientSecret);
                return new OkObjectResult(value);
            }
            catch
            {
                log.LogError("Journey API token error.");
                return new InternalServerErrorResult();
            }
        }
    }
}
