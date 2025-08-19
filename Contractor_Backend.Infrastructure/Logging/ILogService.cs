namespace Contractor_Backend.Infrastructure.Logging
{
    public interface ILogService
    {
        void LogInfo(string message, object? context = null);
        void LogError(Exception ex, string message);
    }
}
