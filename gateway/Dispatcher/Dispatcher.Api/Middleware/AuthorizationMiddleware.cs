using Dispatcher.Application.Logging;
using Dispatcher.Domain.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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

        if (path == "/favicon.ico" || path == "/metrics")
        {
            await _next(context);
            return;
        }

        if (!path.StartsWith("/auth") && !path.StartsWith("/admin"))
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token) || !ValidateToken(token))
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