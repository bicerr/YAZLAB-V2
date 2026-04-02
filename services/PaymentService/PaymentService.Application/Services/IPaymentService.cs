using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Services;

/// <summary>
/// Ödeme iş mantığı için servis sözleşmesi
/// </summary>
public interface IPaymentService
{
    Task<List<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(string id);
    Task<Payment?> GetByOrderIdAsync(string orderId);
    Task<List<Payment>> GetByStatusAsync(string status);
    Task<PaymentResult> CreateAsync(PaymentDto dto);
    Task<bool> CompleteAsync(string id);
    Task<bool> FailAsync(string id);
}