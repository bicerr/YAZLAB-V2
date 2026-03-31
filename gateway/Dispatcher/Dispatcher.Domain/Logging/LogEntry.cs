namespace Dispatcher.Domain.Logging;

public class LogEntry
{
    public string Id { get; private set; }
    public string Method { get; private set; }
    public string Path { get; private set; }
    public int StatusCode { get; private set; }
    public DateTime Timestamp { get; private set; }

    public LogEntry(string method, string path, int statusCode)
    {
        Id = Guid.NewGuid().ToString();
        Method = method;
        Path = path;
        StatusCode = statusCode;
        Timestamp = DateTime.UtcNow;
    }
}