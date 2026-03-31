using OrderService.Application.DTOs;
using OrderService.Application.Repositories;
using OrderService.Application.Services;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Services;

public class OrderServiceImpl : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderServiceImpl(IOrderRepository repository)
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