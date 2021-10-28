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
            var date = DateTime.Today.AddDays(2);
            var availabilities = Task.WhenAll(courts.Select(x => tennisClient.GetTimeSlotListAsync(x, date))).GetAwaiter().GetResult();
        }
    }
}
