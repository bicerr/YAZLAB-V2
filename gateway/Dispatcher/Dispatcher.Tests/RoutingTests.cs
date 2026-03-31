using Dispatcher.Application.Routing;
using Dispatcher.Domain.Routing;

namespace Dispatcher.Tests;

public class RoutingTests
{
    [Fact]
    public async Task GetAllRoutes_ShouldReturnEmpty_WhenNoRoutes()
    {
        IRouteRepository repository = new FakeRouteRepository();
        var result = await repository.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddRoute_ShouldAddRoute_WhenCalled()
    {
        IRouteRepository repository = new FakeRouteRepository();
        var route = new RouteConfig("/products", "http://product-service:8080");
        await repository.AddAsync(route);
        var result = await repository.GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByPath_ShouldReturnRoute_WhenExists()
    {
        IRouteRepository repository = new FakeRouteRepository();
        var route = new RouteConfig("/products", "http://product-service:8080");
        await repository.AddAsync(route);
        var result = await repository.GetByPathAsync("/products");
        Assert.NotNull(result);
    }
}

public class FakeRouteRepository : IRouteRepository
{
    private readonly List<RouteConfig> _routes = new();

    public Task<List<RouteConfig>> GetAllAsync()
        => Task.FromResult(_routes.ToList());

    public Task<RouteConfig?> GetByPathAsync(string path)
        => Task.FromResult(_routes.FirstOrDefault(r => r.Path == path));

    public Task AddAsync(RouteConfig route)
    {
        _routes.Add(route);
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(string id)
    {
        var route = _routes.FirstOrDefault(r => r.Id == id);
        if (route == null) return Task.FromResult(false);
        _routes.Remove(route);
        return Task.FromResult(true);
    }
}