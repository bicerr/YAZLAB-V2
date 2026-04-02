using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Services;

public interface IPaymentService
{
    Task<List<Payment>> GetAllAsync();
    Task<PaymentResult> CreateAsync(PaymentDto dto);
    Task<bool> CompleteAsync(string id);
    Task<bool> FailAsync(string id);
}