using MongoDB.Bson.Serialization.Attributes;

namespace Dispatcher.Domain.Logging;

public class LogEntry
{
    [BsonId]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("Method")]
    public string Method { get; set; } = string.Empty;
    
    [BsonElement("Path")]
    public string Path { get; set; } = string.Empty;
    
    [BsonElement("StatusCode")]
    public int StatusCode { get; set; }
    
    [BsonElement("Timestamp")]
    public DateTime Timestamp { get; set; }
    
    [BsonElement("ClientIp")]
    public string? ClientIp { get; set; }
    
    [BsonElement("ResponseTimeMs")]
    public long ResponseTimeMs { get; set; }

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

    public LogEntry() { }
}