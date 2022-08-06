using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TennisPlanner.Shared.Services.Logging
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger _logger;
        private static Lazy<LoggerService> _lazyLoggerService 
            = new Lazy<LoggerService>(() => new LoggerService());

        public static LoggerService Instance => _lazyLoggerService.Value;

        private LoggerService()
        {
            var loggerFactory = new LoggerFactory();
            _logger = new Logger<LoggerService>(loggerFactory);
        }

        public void Log(LogLevel logLevel, string operationName, string message, Exception? exception = null)
        {
#if DEBUG
            var logLine = JsonSerializer.Serialize(new
            {
                operationName = operationName,
                message = message,
                exception = exception,
            });

            switch (logLevel)
            {
                case LogLevel.Error:
                case LogLevel.Warning:
                    Console.Error.WriteLine(logLine);
                    break;
                default:
                    Console.WriteLine(logLine);
                    break;
            };

#endif
            _logger.Log(
                logLevel: logLevel, 
                message: $"{message} in {operationName}.", 
                exception: exception);
        }
    }
}
