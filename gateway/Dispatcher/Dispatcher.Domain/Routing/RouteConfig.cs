namespace Dispatcher.Domain.Routing;

public class RouteConfig
{
    public string Id { get; private set; }
    public string Path { get; private set; }
    public string Target { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public RouteConfig(string path, string target)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentException.ThrowIfNullOrEmpty(target);

        Id = Guid.NewGuid().ToString();
        Path = path;
        Target = target;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}