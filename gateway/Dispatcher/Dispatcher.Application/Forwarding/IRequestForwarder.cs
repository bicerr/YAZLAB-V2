namespace Dispatcher.Application.Forwarding;

public record ForwardResult(bool IsSuccess, int StatusCode, string? Content);

public interface IRequestForwarder
{
    Task<ForwardResult> ForwardAsync(string method, string targetUrl, string? body);
}