using Dispatcher.Application.Routing;
using Dispatcher.Domain.Routing;
using MongoDB.Driver;

namespace Dispatcher.Infrastructure.Routing;

public class MongoRouteRepository : IRouteRepository
{
    private readonly IMongoCollection<RouteConfig> _routes;

    public MongoRouteRepository(IMongoCollection<RouteConfig> routes)
    {
        _routes = routes;
    }

    public async Task<List<RouteConfig>> GetAllAsync()
        => await _routes.Find(_ => true).ToListAsync();

    public async Task<List<RouteConfig>> GetActiveAsync()
        => await _routes.Find(r => r.IsActive).ToListAsync();

    public async Task<RouteConfig?> GetByPathAsync(string path)
        => await _routes.Find(r => r.Path == path).FirstOrDefaultAsync();

    public async Task AddAsync(RouteConfig route)
        => await _routes.InsertOneAsync(route);

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _routes.DeleteOneAsync(r => r.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> UpdateAsync(RouteConfig route)
    {
        var result = await _routes.ReplaceOneAsync(r => r.Id == route.Id, route);
        return result.MatchedCount > 0;
    }
}