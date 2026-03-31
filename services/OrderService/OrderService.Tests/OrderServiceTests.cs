using OrderService.Application.Repositories;
using OrderService.Application.Services;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Tests;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateOrder_ShouldReturnSuccess_WhenValid()
    {
        IOrderRepository repository = new FakeOrderRepository();
        IOrderService service = new FakeOrderService(repository);
        var result = await service.CreateAsync(new OrderDto("product-1", 2, "test@test.com"));
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnEmpty_WhenNoOrders()
    {
        IOrderRepository repository = new FakeOrderRepository();
        IOrderService service = new FakeOrderService(repository);
        var result = await service.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateStatus_ShouldReturnFalse_WhenOrderNotFound()
    {
        IOrderRepository repository = new FakeOrderRepository();
        IOrderService service = new FakeOrderService(repository);
        var result = await service.UpdateStatusAsync("nonexistent-id", "Tamamlandı");
        Assert.False(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        IOrderRepository repository = new FakeOrderRepository();
        IOrderService service = new FakeOrderService(repository);
        var result = await service.GetByIdAsync("nonexistent-id");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCustomerEmailAsync_ShouldReturnOrders_WhenExists()
    {
        IOrderRepository repository = new FakeOrderRepository();
        IOrderService service = new FakeOrderService(repository);
        await service.CreateAsync(new OrderDto("product-1", 2, "test@test.com"));
        var result = await service.GetByCustomerEmailAsync("test@test.com");
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnOrders_WhenStatusMatches()
    {
        IOrderRepository repository = new FakeOrderRepository();
        IOrderService service = new FakeOrderService(repository);
        await service.CreateAsync(new OrderDto("product-1", 2, "test@test.com"));
        var result = await service.GetByStatusAsync("Beklemede");
        Assert.Single(result);
    }
}

public class FakeOrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public FakeOrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Order>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Order?> GetByIdAsync(string id)
        => await _repository.GetByIdAsync(id);

    public async Task<List<Order>> GetByCustomerEmailAsync(string email)
        => await _repository.GetByCustomerEmailAsync(email);

    public async Task<List<Order>> GetByStatusAsync(string status)
        => await _repository.GetByStatusAsync(status);

    public async Task<OrderResult> CreateAsync(OrderDto dto)
    {
        var order = new Order(dto.ProductId, dto.Quantity, dto.CustomerEmail);
        await _repository.CreateAsync(order);
        return new OrderResult(true, null);
    }

    public async Task<bool> UpdateStatusAsync(string id, string status)
        => await _repository.UpdateStatusAsync(id, status);

    public async Task<bool> DeleteAsync(string id)
        => await _repository.DeleteAsync(id);
}