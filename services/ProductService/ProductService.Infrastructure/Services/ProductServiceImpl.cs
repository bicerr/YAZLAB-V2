using ProductService.Application.DTOs;
using ProductService.Application.Repositories;
using ProductService.Application.Services;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Services;

public class ProductServiceImpl : IProductService
{
    private readonly IProductRepository _repository;

    public ProductServiceImpl(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Product>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Product?> GetByIdAsync(string id)
        => await _repository.GetByIdAsync(id);

    public async Task<List<Product>> GetByCategoryAsync(string category)
        => await _repository.GetByCategoryAsync(category);

    public async Task<ServiceResult> CreateAsync(ProductDto dto)
    {
        var product = new Product(dto.Name, dto.Price, dto.Stock, dto.Category);
        await _repository.CreateAsync(product);
        return new ServiceResult(true, null);
    }

    public async Task<ServiceResult> UpdateAsync(string id, ProductDto dto)
    {
        var exists = await _repository.ExistsAsync(id);
        if (!exists) return new ServiceResult(false, "Ürün bulunamadı.");
        var product = new Product(dto.Name, dto.Price, dto.Stock, dto.Category);
        await _repository.UpdateAsync(id, product);
        return new ServiceResult(true, null);
    }

    public async Task<bool> DeleteAsync(string id)
        => await _repository.DeleteAsync(id);
}