using MongoDB.Driver;
using OrderService.Application.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Repositories;

public class MongoOrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public MongoOrderRepository(IMongoCollection<Order> orders)
    {
        _orders = orders;
    }

    public async Task<List<Order>> GetAllAsync()
        => await _orders.Find(_ => true).ToListAsync();

    public async Task<Order?> GetByIdAsync(string id)
        => await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();

    public async Task<List<Order>> GetByCustomerEmailAsync(string email)
        => await _orders.Find(o => o.CustomerEmail == email).ToListAsync();

    public async Task<List<Order>> GetByStatusAsync(string status)
        => await _orders.Find(o => o.Status == status).ToListAsync();

    public async Task CreateAsync(Order order)
        => await _orders.InsertOneAsync(order);

    public async Task<bool> UpdateStatusAsync(string id, string status)
    {
        var order = await GetByIdAsync(id);
        if (order == null) return false;
        order.UpdateStatus(status);
        var result = await _orders.ReplaceOneAsync(o => o.Id == id, order);
        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _orders.DeleteOneAsync(o => o.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var count = await _orders.CountDocumentsAsync(o => o.Id == id);
        return count > 0;
    }
}