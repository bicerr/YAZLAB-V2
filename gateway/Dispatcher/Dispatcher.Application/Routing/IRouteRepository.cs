using Dispatcher.Domain.Routing;

namespace Dispatcher.Application.Routing;

public interface IRouteRepository
{
    Task<List<RouteConfig>> GetAllAsync();
    Task<RouteConfig?> GetByPathAsync(string path);
    Task AddAsync(RouteConfig route);
    Task<bool> DeleteAsync(string id);
}