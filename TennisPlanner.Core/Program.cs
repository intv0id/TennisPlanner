using System;
using System.Linq;
using System.Threading.Tasks;
using TennisPlanner.Core.Clients;

namespace TennisPlanner.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var tennisClient = new TennisParisClient();
            var courts = tennisClient.GetTennisCourtsListAsync().GetAwaiter().GetResult();
            var date = DateTime.Today.AddDays(0);
            var availabilities = Task.WhenAll(courts.Select(court => tennisClient.GetTimeSlotListAsync(court, date)))
                .GetAwaiter()
                .GetResult()
                .SelectMany(x => x)
                .ToList();

            Console.WriteLine(availabilities);
        }
    }
}
