using Dispatcher.Application.Forwarding;

namespace Dispatcher.Infrastructure.Http;

public class HttpRequestForwarder : IRequestForwarder
{
    private readonly HttpClient _httpClient;

    public HttpRequestForwarder(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ForwardResult> ForwardAsync(string method, string targetUrl, string? body)
    {
        try
        {
            var request = new HttpRequestMessage(new HttpMethod(method), targetUrl);
            if (body != null)
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return new ForwardResult(response.IsSuccessStatusCode, (int)response.StatusCode, content);
        }
        catch (Exception ex)
        {
            return new ForwardResult(false, 503, null, ex.Message);
        }
    }

    public async Task<ForwardResult> ForwardWithHeadersAsync(string method, string targetUrl, string? body, Dictionary<string, string> headers)
    {
        try
        {
            var request = new HttpRequestMessage(new HttpMethod(method), targetUrl);
            if (body != null)
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

            foreach (var header in headers)
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return new ForwardResult(response.IsSuccessStatusCode, (int)response.StatusCode, content);
        }
        catch (Exception ex)
        {
            return new ForwardResult(false, 503, null, ex.Message);
        }
    }
}