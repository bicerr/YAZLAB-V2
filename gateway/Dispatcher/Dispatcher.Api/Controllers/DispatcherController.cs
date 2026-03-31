using Dispatcher.Application.Forwarding;
using Dispatcher.Application.Logging;
using Dispatcher.Application.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.Api.Controllers;

[ApiController]
[Route("api/dispatcher")]
public class DispatcherController : ControllerBase
{
    private readonly ILogRepository _logRepository;
    private readonly IRouteRepository _routeRepository;

    public DispatcherController(ILogRepository logRepository, IRouteRepository routeRepository)
    {
        _logRepository = logRepository;
        _routeRepository = routeRepository;
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs()
    {
        var logs = await _logRepository.GetRecentAsync(100);
        return Ok(logs);
    }

    [HttpGet("logs/status/{statusCode}")]
    public async Task<IActionResult> GetLogsByStatus(int statusCode)
    {
        var logs = await _logRepository.GetByStatusCodeAsync(statusCode);
        return Ok(logs);
    }

    [HttpGet("routes")]
    public async Task<IActionResult> GetRoutes()
    {
        var routes = await _routeRepository.GetAllAsync();
        return Ok(routes);
    }

    [HttpPost("routes")]
    public async Task<IActionResult> AddRoute([FromBody] RouteRequest request)
    {
        var route = new Dispatcher.Domain.Routing.RouteConfig(request.Path, request.Target);
        await _routeRepository.AddAsync(route);
        return Created("", route);
    }

    [HttpDelete("routes/{id}")]
    public async Task<IActionResult> DeleteRoute(string id)
    {
        var result = await _routeRepository.DeleteAsync(id);
        if (!result) return NotFound("Route bulunamadı.");
        return Ok("Route silindi.");
    }
}

public record RouteRequest(string Path, string Target);