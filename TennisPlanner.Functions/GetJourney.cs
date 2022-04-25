using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using TennisPlanner.Core.Clients;
using TennisPlanner.Functions.Configuration;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.Functions;

public static class GetJourney
{
    [FunctionName("GetJourney")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        var configurationProvider = new FunctionsConfigurationProvider();
        var (clientId, clientSecret) = configurationProvider.GetIdfMobiliteClientCredentials();
        var transportClient = new IdfMobilitesClient(clientId: clientId, clientSecret: clientSecret);

        var requestDto = await JsonSerializer.DeserializeAsync<JourneyRequestDto>(req.Body);

        try
        {
            var value = await transportClient.GetTransportationJourneyAsync(
                arrivalTime: requestDto.ArrivalDateTime,
                fromGeoCoordinates: requestDto.FromGeoCoordinates,
                toGeoCoordinates: requestDto.ToGeoCoordinates);
            return new OkObjectResult(JsonSerializer.Serialize(value));
        }
        catch
        {
            log.LogInformation("Journey API error.");
            return new InternalServerErrorResult();
        }
    }
}
