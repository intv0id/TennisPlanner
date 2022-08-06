using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using TennisPlanner.Core.Clients;
using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Core.Exceptions;
using TennisPlanner.Functions.Configuration;

namespace TennisPlanner.Functions
{
    public static class GetJourney
    {
        [FunctionName("GetJourney")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var configurationProvider = new FunctionsConfigurationProvider();
            var apiToken = configurationProvider.GetPrimToken();
            var transportClient = new PrimClient(apiToken: apiToken);

            try
            {
                var transportationJourneyRequest = JsonSerializer.Deserialize<TransportationJourneyRequestDto>(req.Body);
                var value = await transportClient.GetTransportationJourneyAsync(
                    arrivalTime: transportationJourneyRequest.ArrivalTime,
                    fromGeoCoordinates: transportationJourneyRequest.FromGeoCoordinates,
                    toGeoCoordinates: transportationJourneyRequest.ToGeoCoordinates);
                
                return new OkObjectResult(value);
            }
            catch (TransportClientException ex)
            {
                log.LogError(exception: ex, message: "Client exception.");
                return new InternalServerErrorResult();
            }
            catch (Exception ex)
            {
                log.LogError(exception: ex, message: "Unexpected error.");
                return new InternalServerErrorResult();
            }
        }
    }
}
