namespace Dispatcher.Domain.Logging;

public class LogEntry
{
    public string Id { get; private set; }
    public string Method { get; private set; }
    public string Path { get; private set; }
    public int StatusCode { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string? ClientIp { get; private set; }
    public long ResponseTimeMs { get; private set; }

    public LogEntry(string method, string path, int statusCode, string? clientIp = null, long responseTimeMs = 0)
    {
        ArgumentException.ThrowIfNullOrEmpty(method);
        ArgumentException.ThrowIfNullOrEmpty(path);

        Id = Guid.NewGuid().ToString();
        Method = method;
        Path = path;
        StatusCode = statusCode;
        Timestamp = DateTime.UtcNow;
        ClientIp = clientIp;
        ResponseTimeMs = responseTimeMs;
    }
}