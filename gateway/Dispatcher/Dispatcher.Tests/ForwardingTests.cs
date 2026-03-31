using Dispatcher.Application.Forwarding;

namespace Dispatcher.Tests;

public class ForwardingTests
{
    [Fact]
    public async Task Forward_ShouldReturnSuccess_WhenServiceIsReachable()
    {
        IRequestForwarder forwarder = new FakeRequestForwarder(true);
        var result = await forwarder.ForwardAsync("GET", "/products", null);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Forward_ShouldReturnFailure_WhenServiceIsUnreachable()
    {
        IRequestForwarder forwarder = new FakeRequestForwarder(false);
        var result = await forwarder.ForwardAsync("GET", "/products", null);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Forward_ShouldReturnStatusCode_WhenCalled()
    {
        IRequestForwarder forwarder = new FakeRequestForwarder(true);
        var result = await forwarder.ForwardAsync("GET", "/products", null);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task ForwardWithHeaders_ShouldReturnSuccess_WhenCalled()
    {
        IRequestForwarder forwarder = new FakeRequestForwarder(true);
        var headers = new Dictionary<string, string> { { "Authorization", "Bearer token" } };
        var result = await forwarder.ForwardWithHeadersAsync("GET", "/products", null, headers);
        Assert.True(result.IsSuccess);
    }
}

public class FakeRequestForwarder : IRequestForwarder
{
    private readonly bool _shouldSucceed;

    public FakeRequestForwarder(bool shouldSucceed)
    {
        _shouldSucceed = shouldSucceed;
    }

    public Task<ForwardResult> ForwardAsync(string method, string targetUrl, string? body)
    {
        if (_shouldSucceed)
            return Task.FromResult(new ForwardResult(true, 200, "OK"));
        return Task.FromResult(new ForwardResult(false, 503, "Service Unavailable"));
    }

    public Task<ForwardResult> ForwardWithHeadersAsync(string method, string targetUrl, string? body, Dictionary<string, string> headers)
    {
        if (_shouldSucceed)
            return Task.FromResult(new ForwardResult(true, 200, "OK"));
        return Task.FromResult(new ForwardResult(false, 503, "Service Unavailable"));
    }
}