using Dispatcher.Domain.Routing;

namespace Dispatcher.Tests;

public class RouteConfigTests
{
    [Fact]
    public void RouteConfig_ShouldHavePath_WhenCreated()
    {
        var route = new RouteConfig("/products", "http://product-service:8080");
        Assert.Equal("/products", route.Path);
    }

    [Fact]
    public void RouteConfig_ShouldHaveTarget_WhenCreated()
    {
        var route = new RouteConfig("/products", "http://product-service:8080");
        Assert.Equal("http://product-service:8080", route.Target);
    }

    [Fact]
    public void RouteConfig_ShouldBeActive_WhenCreated()
    {
        var route = new RouteConfig("/products", "http://product-service:8080");
        Assert.True(route.IsActive);
    }
}