namespace TennisPlanner.Server.Services
{
    public interface INotificationService
    {
        public void Display(LogLevel level, string message);
    }
}
