using Dispatcher.Domain.Logging;

namespace Dispatcher.Application.Logging;


public interface ILogRepository
{
    Task SaveAsync(LogEntry log);
    Task<List<LogEntry>> GetAllAsync();
    Task<List<LogEntry>> GetRecentAsync(int count);
    Task<List<LogEntry>> GetByStatusCodeAsync(int statusCode);
    Task<int> GetTotalCountAsync();
}