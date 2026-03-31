using OrderService.Application.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<OrderResult> CreateAsync(OrderDto dto);
    Task<bool> UpdateStatusAsync(string id, string status);
    Task<bool> DeleteAsync(string id);
}