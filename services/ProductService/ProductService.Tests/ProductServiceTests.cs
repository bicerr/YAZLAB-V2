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

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        IProductRepository repository = new FakeProductRepository();
        IProductService service = new FakeProductService(repository);
        var result = await service.GetByIdAsync("nonexistent-id");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCategoryAsync_ShouldReturnProducts_WhenCategoryMatches()
    {
        IProductRepository repository = new FakeProductRepository();
        IProductService service = new FakeProductService(repository);
        await service.CreateAsync(new ProductDto("Laptop", 15000, 50, "Elektronik"));
        var result = await service.GetByCategoryAsync("Elektronik");
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnSuccess_WhenProductExists()
    {
        IProductRepository repository = new FakeProductRepository();
        IProductService service = new FakeProductService(repository);
        await service.CreateAsync(new ProductDto("Laptop", 15000, 50, "Elektronik"));
        var products = await service.GetAllAsync();
        var result = await service.UpdateAsync(products[0].Id, new ProductDto("Laptop Pro", 20000, 30, "Elektronik"));
        Assert.True(result.Success);
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
        var product = new Product(dto.Name, dto.Price, dto.Stock, dto.Category);
        var result = await _repository.UpdateAsync(id, product);
        return new ServiceResult(result, result ? null : "Ürün bulunamadı.");
    }

    public async Task<bool> DeleteAsync(string id)
        => await _repository.DeleteAsync(id);
}