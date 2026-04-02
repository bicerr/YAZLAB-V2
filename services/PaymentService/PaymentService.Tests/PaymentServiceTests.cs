using PaymentService.Application.Repositories;
using PaymentService.Application.Services;
using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;

namespace PaymentService.Tests;

public class PaymentServiceTests
{
    [Fact]
    public async Task CreatePayment_ShouldReturnSuccess_WhenValid()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        IPaymentService service = new FakePaymentService(repository);
        var result = await service.CreateAsync(new PaymentDto("order-1", 150.00m, "CreditCard"));
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetAllPayments_ShouldReturnEmpty_WhenNoPayments()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        IPaymentService service = new FakePaymentService(repository);
        var result = await service.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task CompletePayment_ShouldReturnFalse_WhenNotFound()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        IPaymentService service = new FakePaymentService(repository);
        var result = await service.CompleteAsync("nonexistent-id");
        Assert.False(result);
    }

    [Fact]
    public async Task GetByOrderId_ShouldReturnPayment_WhenExists()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        IPaymentService service = new FakePaymentService(repository);
        await service.CreateAsync(new PaymentDto("order-1", 150.00m, "CreditCard"));
        var result = await service.GetByOrderIdAsync("order-1");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByStatus_ShouldReturnPayments_WhenStatusMatches()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        IPaymentService service = new FakePaymentService(repository);
        await service.CreateAsync(new PaymentDto("order-1", 150.00m, "CreditCard"));
        var result = await service.GetByStatusAsync("Pending");
        Assert.Single(result);
    }
}

public class FakePaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;

    public FakePaymentService(IPaymentRepository repository)
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