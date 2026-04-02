using PaymentService.Domain.Entities;

namespace PaymentService.Application.Repositories;

/// <summary>
/// Ödeme veri erişim katmanı için sözleşme
/// </summary>
public interface IPaymentRepository
{
    Task<List<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(string id);
    Task<Payment?> GetByOrderIdAsync(string orderId);
    Task<List<Payment>> GetByStatusAsync(string status);
    Task CreateAsync(Payment payment);
    Task<bool> UpdateAsync(Payment payment);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}