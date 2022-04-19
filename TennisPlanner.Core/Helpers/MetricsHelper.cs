using System;
using System.Collections.Generic;
using System.Linq;

namespace TennisPlanner.Core.Helpers;

/// <summary>
/// Helpers for metric operations.
/// </summary>
public static class MetricsHelper
{
    /// <summary>
    /// Computes L2 metric to minimize for travel time.
    /// </summary>
    /// <param name="durations">A enumerable of journey durations.</param>
    /// <returns>The computed metric.</returns>
    public static double TravelTimeL2Metric(IEnumerable<int?> durations)
    {
        if (durations.Any(d => d == null))
        {
            return double.PositiveInfinity;
        }

        return durations
            .Select(d => d*d)
            .Aggregate(0, (acc, x) => acc + (x ?? throw new ArgumentNullException()));
    }
}
