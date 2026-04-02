using MongoDB.Driver;
using PaymentService.Application.Repositories;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Repositories;

public class MongoPaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _payments;

    public MongoPaymentRepository(IMongoCollection<Payment> payments)
    {
        _payments = payments;
    }

    public async Task<List<Payment>> GetAllAsync()
        => await _payments.Find(_ => true).SortByDescending(p => p.CreatedAt).ToListAsync();

    public async Task<Payment?> GetByIdAsync(string id)
        => await _payments.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task<Payment?> GetByOrderIdAsync(string orderId)
        => await _payments.Find(p => p.OrderId == orderId).FirstOrDefaultAsync();

    public async Task<List<Payment>> GetByStatusAsync(string status)
        => await _payments.Find(p => p.Status == status).ToListAsync();

    public async Task CreateAsync(Payment payment)
        => await _payments.InsertOneAsync(payment);

    public async Task<bool> UpdateAsync(Payment payment)
    {
        var result = await _payments.ReplaceOneAsync(p => p.Id == payment.Id, payment);
        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _payments.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var count = await _payments.CountDocumentsAsync(p => p.Id == id);
        return count > 0;
    }
}