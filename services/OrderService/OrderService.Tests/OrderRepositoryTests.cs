using OrderService.Application.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Tests;

public class OrderRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmpty_WhenNoOrders()
    {
        IOrderRepository repository = new FakeOrderRepository();
        var result = await repository.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddOrder_WhenCalled()
    {
        IOrderRepository repository = new FakeOrderRepository();
        var order = new Order("product-1", 2, "test@test.com");
        await repository.CreateAsync(order);
        var result = await repository.GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        IOrderRepository repository = new FakeOrderRepository();
        var result = await repository.GetByIdAsync("nonexistent-id");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCustomerEmailAsync_ShouldReturnOrders_WhenExists()
    {
        IOrderRepository repository = new FakeOrderRepository();
        var order = new Order("product-1", 2, "test@test.com");
        await repository.CreateAsync(order);
        var result = await repository.GetByCustomerEmailAsync("test@test.com");
        Assert.Single(result);
    }
}

public class FakeOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public Task<List<Order>> GetAllAsync()
        => Task.FromResult(_orders.ToList());

    public Task<Order?> GetByIdAsync(string id)
        => Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));

    public Task<List<Order>> GetByCustomerEmailAsync(string email)
        => Task.FromResult(_orders.Where(o => o.CustomerEmail == email).ToList());

    public Task CreateAsync(Order order)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateStatusAsync(string id, string status)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return Task.FromResult(false);
        order.UpdateStatus(status);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(string id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return Task.FromResult(false);
        _orders.Remove(order);
        return Task.FromResult(true);
    }
}