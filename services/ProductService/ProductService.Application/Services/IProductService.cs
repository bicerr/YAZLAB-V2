using ProductService.Application.DTOs;
using ProductService.Domain.Entities;

namespace ProductService.Application.Services;


public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(string id);
    Task<List<Product>> GetByCategoryAsync(string category);
    Task<ServiceResult> CreateAsync(ProductDto dto);
    Task<ServiceResult> UpdateAsync(string id, ProductDto dto);
    Task<bool> DeleteAsync(string id);
}