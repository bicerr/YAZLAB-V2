using MongoDB.Driver;
using ProductService.Application.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Repositories;

public class MongoProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _products;

    public MongoProductRepository(IMongoCollection<Product> products)
    {
        _products = products;
    }

    public async Task<List<Product>> GetAllAsync()
        => await _products.Find(_ => true).ToListAsync();

    public async Task<Product?> GetByIdAsync(string id)
        => await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task<List<Product>> GetByCategoryAsync(string category)
        => await _products.Find(p => p.Category == category).ToListAsync();

    public async Task CreateAsync(Product product)
        => await _products.InsertOneAsync(product);

    public async Task<bool> UpdateAsync(string id, Product product)
    {
        var result = await _products.ReplaceOneAsync(p => p.Id == id, product);
        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _products.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var count = await _products.CountDocumentsAsync(p => p.Id == id);
        return count > 0;
    }
}