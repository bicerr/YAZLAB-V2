namespace Dispatcher.Application.Forwarding;

public record ForwardResult(bool IsSuccess, int StatusCode, string? Content, string? ErrorMessage = null);


public interface IRequestForwarder
{
    Task<ForwardResult> ForwardAsync(string method, string targetUrl, string? body);
    Task<ForwardResult> ForwardWithHeadersAsync(string method, string targetUrl, string? body, Dictionary<string, string> headers);
}