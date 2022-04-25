using TennisPlanner.Core.Contracts.Transport;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.Shared.Extensions;

/// <summary>
/// Extensions for <see cref="JourneyDto"/>.
/// </summary>
public static class JourneyDtoExtensions
{
    /// <summary>
    /// Converts a <see cref="JourneyDto"/> to a <see cref="Journey"/>
    /// </summary>
    /// <param name="journeyDto">The input journey dto.</param>
    /// <returns>The output journey.</returns>
    public static Journey ToJourney(this JourneyDto journeyDto)
    {
        return new Journey() 
        { 
            TotalDurationInSeconds = journeyDto.JourneyDuration.TotalDurationInSeconds,
            WalkingDurationInSeconds = journeyDto.JourneyDuration.WalkingDurationInSeconds,
        };
    }
}
