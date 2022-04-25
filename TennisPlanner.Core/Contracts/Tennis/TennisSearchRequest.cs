using System;
using System.Text.Json.Serialization;

namespace TennisPlanner.Core.Contracts.Tennis;

public class TennisSearchRequest
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    public TennisSearchRequest(DateTime date)
    {
        this.Date = date;
    }
}
