using Dispatcher.Application.Logging;
using Dispatcher.Domain.Logging;
using MongoDB.Driver;

namespace Dispatcher.Infrastructure.Logging;

public class MongoLogRepository : ILogRepository
{
    private readonly IMongoCollection<LogEntry> _logs;

    public MongoLogRepository(IMongoCollection<LogEntry> logs)
    {
        _logs = logs;
    }

    public async Task SaveAsync(LogEntry log)
        => await _logs.InsertOneAsync(log);

    public async Task<List<LogEntry>> GetAllAsync()
        => await _logs.Find(_ => true).SortByDescending(l => l.Timestamp).ToListAsync();

    public async Task<List<LogEntry>> GetRecentAsync(int count)
        => await _logs.Find(_ => true).SortByDescending(l => l.Timestamp).Limit(count).ToListAsync();

    public async Task<List<LogEntry>> GetByStatusCodeAsync(int statusCode)
        => await _logs.Find(l => l.StatusCode == statusCode).ToListAsync();

    public async Task<int> GetTotalCountAsync()
    {
        var count = await _logs.CountDocumentsAsync(_ => true);
        return (int)count;
    }
}