using Microsoft.Extensions.Logging;

namespace TennisPlanner.Shared.Services.Logging
{
    public interface ILoggerService
    {
        void Log(LogLevel logLevel, string operationName, string message);
    }
}
