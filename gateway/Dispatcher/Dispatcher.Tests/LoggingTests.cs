using Dispatcher.Application.Logging;
using Dispatcher.Domain.Logging;

namespace Dispatcher.Tests;

public class LoggingTests
{
    [Fact]
    public async Task SaveLog_ShouldAddLog_WhenCalled()
    {
        ILogRepository repository = new FakeLogRepository();
        var log = new LogEntry("GET", "/products", 200);
        await repository.SaveAsync(log);
        var result = await repository.GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAllLogs_ShouldReturnEmpty_WhenNoLogs()
    {
        ILogRepository repository = new FakeLogRepository();
        var result = await repository.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRecentLogs_ShouldReturnLimitedLogs_WhenCalled()
    {
        ILogRepository repository = new FakeLogRepository();
        for (int i = 0; i < 10; i++)
            await repository.SaveAsync(new LogEntry("GET", "/products", 200));
        var result = await repository.GetRecentAsync(5);
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task GetByStatusCode_ShouldReturnLogs_WhenStatusMatches()
    {
        ILogRepository repository = new FakeLogRepository();
        await repository.SaveAsync(new LogEntry("GET", "/products", 200));
        await repository.SaveAsync(new LogEntry("GET", "/orders", 404));
        var result = await repository.GetByStatusCodeAsync(200);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetTotalCount_ShouldReturnCorrectCount_WhenCalled()
    {
        ILogRepository repository = new FakeLogRepository();
        await repository.SaveAsync(new LogEntry("GET", "/products", 200));
        await repository.SaveAsync(new LogEntry("POST", "/orders", 201));
        var result = await repository.GetTotalCountAsync();
        Assert.Equal(2, result);
    }
}

public class FakeLogRepository : ILogRepository
{
    private readonly List<LogEntry> _logs = new();

    public Task SaveAsync(LogEntry log)
    {
        _logs.Add(log);
        return Task.CompletedTask;
    }

    public Task<List<LogEntry>> GetAllAsync()
        => Task.FromResult(_logs.ToList());

    public Task<List<LogEntry>> GetRecentAsync(int count)
        => Task.FromResult(_logs.TakeLast(count).ToList());

    public Task<List<LogEntry>> GetByStatusCodeAsync(int statusCode)
        => Task.FromResult(_logs.Where(l => l.StatusCode == statusCode).ToList());

    public Task<int> GetTotalCountAsync()
        => Task.FromResult(_logs.Count);
}