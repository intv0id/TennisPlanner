using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts;

namespace TennisPlanner.Core.Clients
{
    public class TennisParisClient : ITennisClient
    {
        private const string tennisListUrl = 
            "https://tennis.paris.fr/tennis/jsp/site/Portal.jsp?page=tennisParisien&view=les_tennis_parisiens";
        private const string tennisAvalabilitySearch =
            "https://tennis.paris.fr/tennis/jsp/site/Portal.jsp?page=recherche&action=ajax_load_planning";

        public async Task<List<TennisCourt>> GetTennisCourtsListAsync()
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync(tennisListUrl);
            var document = new HtmlDocument();
            document.LoadHtml(content);
            HtmlNode tennisListHtml = document.GetElementbyId("tennisParisiens");
            var tennisListNodes = tennisListHtml.ChildNodes
                .Descendants().Where(x => x.Name == "table" && x.HasClass("tennis-desktop"))
                .SelectMany(x => x.Descendants().Where(x => x.Name == "tbody"))
                .SelectMany(x => x.Descendants().Where(x => x.Name == "tr"));

            var tennisCourtsList = new List<TennisCourt>();
            foreach (var node in tennisListNodes)
            {
                var items = node.Descendants()
                    .Where(x => x.Name == "td")
                    .Select(x => x.InnerText)
                    .ToList();
                var court = new TennisCourt
                {
                    Name = items[1],
                    Adress = items[2],
                    CourtsCount = int.TryParse(items[3], out int count) ? count : -1,
                };

                tennisCourtsList.Add(court);
            }

            return tennisCourtsList;
        }

        public async Task<List<TimeSlot>> GetTimeSlotListAsync(TennisCourt tennisCourt, DateTime day)
        {
            using var client = new HttpClient();
            var httpContent = new List<KeyValuePair<string, string>>
            {
                KeyValuePair.Create("date_selected", $"{day:dd/MM/yyyy}"),
                KeyValuePair.Create( "name_tennis", tennisCourt.Name.ToUpperInvariant().Replace("TENNIS ", "").Replace(" ", "+") ),
            };
            using var req = new HttpRequestMessage(HttpMethod.Post, tennisAvalabilitySearch) { Content = new FormUrlEncodedContent(httpContent) };
            var postRequest = await client.SendAsync(req);
            var content = await postRequest.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(content);

            return new List<TimeSlot>();
        }
    }
}
