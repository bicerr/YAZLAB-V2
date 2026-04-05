using AuthService.Application.DTOs;
using AuthService.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private static readonly Counter LoginCounter = Metrics.CreateCounter(
        "auth_login_total", "Login istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });
    private static readonly Counter RegisterCounter = Metrics.CreateCounter(
        "auth_register_total", "Kayıt istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result.Success)
        {
            RegisterCounter.WithLabels("failure").Inc();
            return Conflict(result.ErrorMessage);
        }
        RegisterCounter.WithLabels("success").Inc();
        return Ok("Kayıt başarılı.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.Success)
        {
            LoginCounter.WithLabels("failure").Inc();
            return Unauthorized(result.ErrorMessage);
        }
        LoginCounter.WithLabels("success").Inc();
        return Ok(new { token = result.Token });
    }
}