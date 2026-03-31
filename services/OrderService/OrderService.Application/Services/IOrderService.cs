using OrderService.Application.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

/// <summary>
/// Sipariş iş mantığı için servis sözleşmesi
/// </summary>
public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(string id);
    Task<List<Order>> GetByCustomerEmailAsync(string email);
    Task<List<Order>> GetByStatusAsync(string status);
    Task<OrderResult> CreateAsync(OrderDto dto);
    Task<bool> UpdateStatusAsync(string id, string status);
    Task<bool> DeleteAsync(string id);
}