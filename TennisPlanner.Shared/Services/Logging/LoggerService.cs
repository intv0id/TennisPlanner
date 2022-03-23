using Microsoft.Extensions.Logging;

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
            _logger.Log(
                logLevel: logLevel, 
                message: $"{message} in {operationName}.", 
                exception: exception);
        }
    }
}
