using Dispatcher.Application.Logging;
using Dispatcher.Domain.Logging;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Dispatcher.Api.Middleware;

public class AuthorizationMiddleware
{
    private static readonly Counter ForwardedRequestsCounter = Metrics.CreateCounter(
        "dispatcher_forwarded_requests_total", "Dispatcher üzerinden yönlendirilen istekler",
        new CounterConfiguration { LabelNames = new[] { "service", "method", "status" } });
    private static readonly Counter AuthFailuresCounter = Metrics.CreateCounter(
        "dispatcher_auth_failures_total", "Dispatcher yetkilendirme hataları");

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

        if (path == "/favicon.ico" || path == "/metrics" || path.StartsWith("/swagger"))
        {
            await _next(context);
            return;
        }

        var sw = Stopwatch.StartNew();
        var clientIp = context.Connection.RemoteIpAddress?.ToString();

        if (!path.StartsWith("/auth") && !path.StartsWith("/admin"))
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token) || !ValidateToken(token))
            {
                sw.Stop();
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                AuthFailuresCounter.Inc();
                ForwardedRequestsCounter.WithLabels(ResolveService(path), context.Request.Method, "401").Inc();
                await _logRepository.SaveAsync(new LogEntry(
                    context.Request.Method,
                    context.Request.Path,
                    401,
                    clientIp,
                    sw.ElapsedMilliseconds));
                return;
            }
        }

        await _next(context);
        sw.Stop();

        var service = ResolveService(path);
        ForwardedRequestsCounter.WithLabels(service, context.Request.Method, context.Response.StatusCode.ToString()).Inc();

        await _logRepository.SaveAsync(new LogEntry(
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            clientIp,
            sw.ElapsedMilliseconds));
    }

    private static string ResolveService(string path)
    {
        if (path.StartsWith("/auth")) return "auth-service";
        if (path.StartsWith("/api/products")) return "product-service";
        if (path.StartsWith("/api/orders")) return "order-service";
        if (path.StartsWith("/api/payments")) return "payment-service";
        if (path.StartsWith("/api/notifications")) return "notification-service";
        if (path.StartsWith("/admin")) return "dispatcher";
        return "unknown";
    }

    private bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}