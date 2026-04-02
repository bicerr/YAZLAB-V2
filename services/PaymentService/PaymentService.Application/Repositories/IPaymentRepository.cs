using PaymentService.Domain.Entities;

namespace PaymentService.Application.Repositories;

public interface IPaymentRepository
{
    Task<List<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(string id);
    Task<Payment?> GetByOrderIdAsync(string orderId);
    Task CreateAsync(Payment payment);
    Task<bool> UpdateAsync(Payment payment);
    Task<bool> DeleteAsync(string id);
}