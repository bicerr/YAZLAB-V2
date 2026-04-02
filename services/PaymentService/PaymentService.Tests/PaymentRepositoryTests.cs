using PaymentService.Application.Repositories;
using PaymentService.Domain.Entities;

namespace PaymentService.Tests;

public class PaymentRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmpty_WhenNoPayments()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        var result = await repository.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddPayment_WhenCalled()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        var payment = new Payment("order-1", 150.00m, "CreditCard");
        await repository.CreateAsync(payment);
        var result = await repository.GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByOrderIdAsync_ShouldReturnPayment_WhenExists()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        var payment = new Payment("order-1", 150.00m, "CreditCard");
        await repository.CreateAsync(payment);
        var result = await repository.GetByOrderIdAsync("order-1");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        var result = await repository.GetByIdAsync("nonexistent-id");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnPayments_WhenStatusMatches()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        var payment = new Payment("order-1", 150.00m, "CreditCard");
        await repository.CreateAsync(payment);
        var result = await repository.GetByStatusAsync("Pending");
        Assert.Single(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenPaymentExists()
    {
        IPaymentRepository repository = new FakePaymentRepository();
        var payment = new Payment("order-1", 150.00m, "CreditCard");
        await repository.CreateAsync(payment);
        var result = await repository.ExistsAsync(payment.Id);
        Assert.True(result);
    }
}

public class FakePaymentRepository : IPaymentRepository
{
    private readonly List<Payment> _payments = new();

    public Task<List<Payment>> GetAllAsync()
        => Task.FromResult(_payments.ToList());

    public Task<Payment?> GetByIdAsync(string id)
        => Task.FromResult(_payments.FirstOrDefault(p => p.Id == id));

    public Task<Payment?> GetByOrderIdAsync(string orderId)
        => Task.FromResult(_payments.FirstOrDefault(p => p.OrderId == orderId));

    public Task<List<Payment>> GetByStatusAsync(string status)
        => Task.FromResult(_payments.Where(p => p.Status == status).ToList());

    public Task CreateAsync(Payment payment)
    {
        _payments.Add(payment);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateAsync(Payment payment)
    {
        var existing = _payments.FirstOrDefault(p => p.Id == payment.Id);
        if (existing == null) return Task.FromResult(false);
        _payments.Remove(existing);
        _payments.Add(payment);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(string id)
    {
        var existing = _payments.FirstOrDefault(p => p.Id == id);
        if (existing == null) return Task.FromResult(false);
        _payments.Remove(existing);
        return Task.FromResult(true);
    }

    public Task<bool> ExistsAsync(string id)
        => Task.FromResult(_payments.Any(p => p.Id == id));
}