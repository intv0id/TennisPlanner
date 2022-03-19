using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TennisPlanner.Core.Contracts;
using TennisPlanner.Core.Enum;

namespace TennisPlanner.Core.Clients
{
    public class TennisParisClient : ITennisClient
    {
        private const string tennisListUrl = 
            "https://tennis.paris.fr/tennis/jsp/site/Portal.jsp?page=tennisParisien&view=les_tennis_parisiens";
        private const string tennisAvalabilitySearch =
            "https://tennis.paris.fr/tennis/jsp/site/Portal.jsp?page=recherche&action=ajax_load_planning";
        private const string timeRangeFormat = "^(?<startHour>[0-9]{1,2}h) - (?<endHour>[0-9]{1,2})h$";

        private static readonly Regex timeRangeRegex = new Regex(@timeRangeFormat, RegexOptions.Compiled);

        public async Task<IEnumerable<TennisFacility>> GetTennisCourtsListAsync()
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

            var tennisCourtsList = new List<TennisFacility>();
            foreach (var node in tennisListNodes)
            {
                var items = node.Descendants()
                    .Where(x => x.Name == "td")
                    .Select(x => x.InnerText)
                    .ToList();
                var court = new TennisFacility
                {
                    Name = items[1],
                    Adress = items[2],
                    CourtsCount = int.TryParse(items[3], out int count) ? count : -1,
                };

                tennisCourtsList.Add(court);
            }

            return tennisCourtsList;
        }

        public async Task<IEnumerable<TimeSlot>> GetTimeSlotListAsync(TennisFacility tennisFacility, DateTime day)
        {
            using var client = new HttpClient();
            var httpContent = new List<KeyValuePair<string, string>>
            {
                KeyValuePair.Create("date_selected", $"{day:dd/MM/yyyy}"),
                KeyValuePair.Create( "name_tennis", tennisFacility.Name.ToUpperInvariant().Replace("TENNIS ", "").Replace(" ", "+") ),
            };
            using var req = new HttpRequestMessage(HttpMethod.Post, tennisAvalabilitySearch) { Content = new FormUrlEncodedContent(httpContent) };
            var postRequest = await client.SendAsync(req);
            var content = await postRequest.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(content);

            var rootNode = document.DocumentNode.SelectNodes("/table[@class=\"reservation\"]")?.FirstOrDefault();

            if (rootNode == null)
            {
                // TODO: log error
                return new List<TimeSlot>();
            }

            var courtsHtml = rootNode?.Descendants()
                .Where(x => x.Name == "thead")
                .SelectMany(x => x.ChildNodes.Where(x => x.Name == "tr"))
                .SelectMany(x => x.ChildNodes.Where(x => x.Name == "th" && x.HasClass("sorttable_nosort")));

            var courtsInfo = courtsHtml
                .Select(x => getCourtInfo(tennisFacility, x))
                .ToList();

            var courtsCount = courtsHtml.Count();

            var availabilitiesHtml = rootNode?.Descendants()
                .Where(x => x.Name == "tbody")
                .SelectMany(x => x.ChildNodes.Where(x => x.Name == "tr"))
                .Select(x => x.ChildNodes);

            var availabilities = new List<TimeSlot>();

            for (int i = 0 ; i < courtsCount; i++)
            {
                availabilities.AddRange(
                    availabilitiesHtml.Select(x => new TimeSlot(
                        timeRange: extractTimeRange(x.FirstOrDefault(y => y.Name == "td")?.InnerText), 
                        status: reservationCellToStatus(x.Where(y => y.Name == "td").ToList()[i+1]),
                        courtInfo: courtsInfo[i])));
            }

            return availabilities.Where(x => x.Status == CourtStatus.Available).ToList();
        }

        private TennisCourt getCourtInfo(TennisFacility facility, HtmlNode x)
        {
            var title = x.Descendants().Where(x => x.HasClass("title")).FirstOrDefault()?.InnerText;
            var chacteristics = x.Descendants()
                .Where(x => x.Name == "a")
                .FirstOrDefault()
                ?.GetAttributeValue("data-content", "")
                .Split("</span><span>")
                .Select(x => x.Replace("<span>", "").Replace("</span>", ""))
                .ToList();

            return new TennisCourt(
                facility: facility, 
                title: title, 
                roof: chacteristics[0], 
                ground: chacteristics[1], 
                light: chacteristics[2]);
        }

        private TimeRange extractTimeRange(string? timeRange)
        {
            if (timeRange == null)
            {
                throw new ArgumentException("Time range is null");
            }

            var match = timeRangeRegex.Match(timeRange);

            if (!match.Success)
            {
                throw new FormatException("Cannot extract time range: match failed");
            }

            var startHourMatch = match.Groups.GetValueOrDefault("startHour");
            var endHourMatch = match.Groups.GetValueOrDefault("endHour");

            if (startHourMatch == null || endHourMatch == null)
            {
                throw new FormatException("Cannot extract time range: match is null");
            }

            var startHour = int.Parse(startHourMatch.Value);
            var endHour = int.Parse(endHourMatch.Value);

            return new TimeRange(
                startHour: startHour,
                endHour: endHour);
        }
        private CourtStatus reservationCellToStatus(HtmlNode htmlNode)
        {
            var availableSlot = htmlNode.InnerText.Contains("LIBRE");
            return availableSlot ? CourtStatus.Available : CourtStatus.Busy ;
        }
    }
}
