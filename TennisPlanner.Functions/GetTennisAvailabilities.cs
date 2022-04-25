using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TennisPlanner.Core.Clients;
using TennisPlanner.Shared.Helpers;

namespace TennisPlanner.Functions;

public static class GetTennisAvailabilities
{
    [FunctionName("GetTennisAvailabilities")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Request received.");

        var queryParams = req.GetQueryParameterDictionary();

        if (queryParams.TryGetValue(Constants.DateTimeQueryKey, out string dateString) 
            && DateTime.TryParse(dateString, out var date))
        {
            var tennisClient = new TennisParisClient(log);
            var courts = await tennisClient.GetTennisFacilitiesListAsync();
            var availabilities = await Task.WhenAll(courts.Select(async court =>
            await tennisClient.GetTimeSlotListAsync(
                tennisFacility: court,
                day: date)));
            var tennisSlots = availabilities.SelectMany(x => x).ToList();

            return new OkObjectResult(JsonSerializer.Serialize(tennisSlots));
        }

        log.LogInformation("Malformed request.");
        return new BadRequestResult();
    }
}
