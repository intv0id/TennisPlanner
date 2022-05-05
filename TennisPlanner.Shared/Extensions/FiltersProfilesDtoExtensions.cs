using System.Text.Json;
using TennisPlanner.Shared.Exceptions;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.Shared.Extensions;

public static class FiltersProfilesDtoExtensions
{
    public static string ToJson(this FiltersProfileDto filtersProfile)
    {
        return JsonSerializer.Serialize(filtersProfile);
    }

    public static FiltersProfileDto FromJson(this string filtersProfile)
    {
        return JsonSerializer.Deserialize<FiltersProfileDto>(filtersProfile)
            ?? throw new PlatformException();
    }
}

