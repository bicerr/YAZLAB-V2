using ProductService.Domain.Entities;

namespace ProductService.Application.Repositories;


public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(string id);
    Task<List<Product>> GetByCategoryAsync(string category);
    Task CreateAsync(Product product);
    Task<bool> UpdateAsync(string id, Product product);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}