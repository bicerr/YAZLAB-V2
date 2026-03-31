using Dispatcher.Application.Logging;
using Dispatcher.Domain.Logging;

namespace Dispatcher.Api.Middleware;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogRepository _logRepository;
    private readonly string _jwtKey = "supersecretkey1234567890abcdefgh";

    public AuthorizationMiddleware(RequestDelegate next, ILogRepository logRepository)
    {
        _next = next;
        _logRepository = logRepository;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";
        var startTime = DateTime.UtcNow;

        if (!path.StartsWith("/auth"))
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");

                await _logRepository.SaveAsync(new LogEntry(
                    context.Request.Method,
                    context.Request.Path,
                    401));
                return;
            }
        }

        await _next(context);

        await _logRepository.SaveAsync(new LogEntry(
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode));
    }
}