using System.Text;

namespace TennisPlanner.Shared.Extensions;

public static class StringExtensions
{
    public static string ToBase64(this string input)
    {
        var byteEncoded = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(byteEncoded);
    }

    public static string FromBase64(this string input)
    {
        var byteEncoded = Convert.FromBase64String(input);
        return Encoding.UTF8.GetString(byteEncoded);
    }
}
