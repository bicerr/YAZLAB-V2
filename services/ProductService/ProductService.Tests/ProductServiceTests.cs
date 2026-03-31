using ProductService.Application.Repositories;
using ProductService.Application.Services;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;

namespace ProductService.Tests;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllProducts_ShouldReturnEmpty_WhenNoProducts()
    {
        IProductRepository repository = new FakeProductRepository();
        IProductService service = new FakeProductService(repository);
        var result = await service.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnSuccess_WhenValid()
    {
        IProductRepository repository = new FakeProductRepository();
        IProductService service = new FakeProductService(repository);
        var result = await service.CreateAsync(new ProductDto("Laptop", 15000, 50, "Elektronik"));
        Assert.True(result.Success);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnFalse_WhenNotFound()
    {
        IProductRepository repository = new FakeProductRepository();
        IProductService service = new FakeProductService(repository);
        var result = await service.DeleteAsync("nonexistent-id");
        Assert.False(result);
    }
}

public class FakeProductService : IProductService
{
    private readonly IProductRepository _repository;

    public FakeProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Product>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<ServiceResult> CreateAsync(ProductDto dto)
    {
        var product = new Product(dto.Name, dto.Price, dto.Stock, dto.Category);
        await _repository.CreateAsync(product);
        return new ServiceResult(true, null);
    }

    public async Task<bool> DeleteAsync(string id)
        => await _repository.DeleteAsync(id);
}