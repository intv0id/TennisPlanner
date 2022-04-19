﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace TennisPlanner.Shared.Helpers
{
    public static class NavigationManagerExtensions
    {
        public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T value)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
            {
                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    value = (T)(object)valueAsInt;
                    return true;
                }
                else if (typeof(T) == typeof(string))
                {
                    value = (T)(object)valueFromQueryString.ToString();
                    return true;
                }
                else if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
                {
                    value = (T)(object)valueAsDecimal;
                    return true;
                }
                else if (typeof(T) == typeof(DateTime) && DateTime.TryParse(valueFromQueryString, out var valueAsDateTime))
                {
                    value = (T)(object)valueAsDateTime;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
