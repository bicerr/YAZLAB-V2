using ProductService.Application.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Tests;

public class ProductRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmpty_WhenNoProducts()
    {
        IProductRepository repository = new FakeProductRepository();
        var result = await repository.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct_WhenCalled()
    {
        IProductRepository repository = new FakeProductRepository();
        var product = new Product("Laptop", 15000, 50, "Elektronik");
        await repository.CreateAsync(product);
        var result = await repository.GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        IProductRepository repository = new FakeProductRepository();
        var result = await repository.GetByIdAsync("nonexistent-id");
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProduct_WhenExists()
    {
        IProductRepository repository = new FakeProductRepository();
        var product = new Product("Laptop", 15000, 50, "Elektronik");
        await repository.CreateAsync(product);
        await repository.DeleteAsync(product.Id);
        var result = await repository.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByCategoryAsync_ShouldReturnProducts_WhenCategoryMatches()
    {
        IProductRepository repository = new FakeProductRepository();
        var product = new Product("Laptop", 15000, 50, "Elektronik");
        await repository.CreateAsync(product);
        var result = await repository.GetByCategoryAsync("Elektronik");
        Assert.Single(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenProductExists()
    {
        IProductRepository repository = new FakeProductRepository();
        var product = new Product("Laptop", 15000, 50, "Elektronik");
        await repository.CreateAsync(product);
        var result = await repository.ExistsAsync(product.Id);
        Assert.True(result);
    }
}

public class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    public Task<List<Product>> GetAllAsync()
        => Task.FromResult(_products.ToList());

    public Task<Product?> GetByIdAsync(string id)
        => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<List<Product>> GetByCategoryAsync(string category)
        => Task.FromResult(_products.Where(p => p.Category == category).ToList());

    public Task CreateAsync(Product product)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateAsync(string id, Product product)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing == null) return Task.FromResult(false);
        _products.Remove(existing);
        _products.Add(product);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(string id)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing == null) return Task.FromResult(false);
        _products.Remove(existing);
        return Task.FromResult(true);
    }

    public Task<bool> ExistsAsync(string id)
        => Task.FromResult(_products.Any(p => p.Id == id));
}