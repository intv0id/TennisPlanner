using System.Runtime.Serialization;

namespace TennisPlanner.Shared.Exceptions;

public class ApiException : Exception
{
    public readonly string ApiName;

    public ApiException(string apiName) : base()
    {
        ApiName = apiName;
    }

    public ApiException(string apiName, string? message) : base(message)
    {
        ApiName = apiName;
    }

    public ApiException(
        string apiName, 
        string? message, 
        Exception? innerException) : base(message, innerException)
    {
        ApiName = apiName;
    }

    protected ApiException(string apiName, SerializationInfo info, StreamingContext context) : base(info, context)
    {
        ApiName = apiName;
    }
}
