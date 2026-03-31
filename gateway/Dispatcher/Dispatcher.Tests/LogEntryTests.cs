using Dispatcher.Domain.Logging;

namespace Dispatcher.Tests;

public class LogEntryTests
{
    [Fact]
    public void LogEntry_ShouldHaveMethod_WhenCreated()
    {
        var log = new LogEntry("GET", "/products", 200);
        Assert.Equal("GET", log.Method);
    }

    [Fact]
    public void LogEntry_ShouldHavePath_WhenCreated()
    {
        var log = new LogEntry("GET", "/products", 200);
        Assert.Equal("/products", log.Path);
    }

    [Fact]
    public void LogEntry_ShouldHaveStatusCode_WhenCreated()
    {
        var log = new LogEntry("GET", "/products", 200);
        Assert.Equal(200, log.StatusCode);
    }

    [Fact]
    public void LogEntry_ShouldHaveTimestamp_WhenCreated()
    {
        var log = new LogEntry("GET", "/products", 200);
        Assert.True(log.Timestamp <= DateTime.UtcNow);
    }
}