using ProductService.Application.DTOs;
using ProductService.Domain.Entities;

namespace ProductService.Application.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<ServiceResult> CreateAsync(ProductDto dto);
    Task<bool> DeleteAsync(string id);
}