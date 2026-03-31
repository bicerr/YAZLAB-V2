using Dispatcher.Domain.Routing;

namespace Dispatcher.Application.Routing;


public interface IRouteRepository
{
    Task<List<RouteConfig>> GetAllAsync();
    Task<List<RouteConfig>> GetActiveAsync();
    Task<RouteConfig?> GetByPathAsync(string path);
    Task AddAsync(RouteConfig route);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(RouteConfig route);
}