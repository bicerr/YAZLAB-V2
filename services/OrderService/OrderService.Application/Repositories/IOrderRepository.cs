using OrderService.Domain.Entities;

namespace OrderService.Application.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(string id);
    Task<List<Order>> GetByCustomerEmailAsync(string email);
    Task CreateAsync(Order order);
    Task<bool> UpdateStatusAsync(string id, string status);
    Task<bool> DeleteAsync(string id);
}