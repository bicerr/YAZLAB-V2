using PaymentService.Application.DTOs;
using PaymentService.Application.Repositories;
using PaymentService.Application.Services;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Services;

public class PaymentServiceImpl : IPaymentService
{
    private readonly IPaymentRepository _repository;

    public PaymentServiceImpl(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Payment>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Payment?> GetByIdAsync(string id)
        => await _repository.GetByIdAsync(id);

    public async Task<Payment?> GetByOrderIdAsync(string orderId)
        => await _repository.GetByOrderIdAsync(orderId);

    public async Task<List<Payment>> GetByStatusAsync(string status)
        => await _repository.GetByStatusAsync(status);

    public async Task<PaymentResult> CreateAsync(PaymentDto dto)
    {
        var payment = new Payment(dto.OrderId, dto.Amount, dto.PaymentMethod);
        await _repository.CreateAsync(payment);
        return new PaymentResult(true, null);
    }

    public async Task<bool> CompleteAsync(string id)
    {
        var payment = await _repository.GetByIdAsync(id);
        if (payment == null) return false;
        payment.Complete();
        return await _repository.UpdateAsync(payment);
    }

    public async Task<bool> FailAsync(string id)
    {
        var payment = await _repository.GetByIdAsync(id);
        if (payment == null) return false;
        payment.Fail();
        return await _repository.UpdateAsync(payment);
    }
}