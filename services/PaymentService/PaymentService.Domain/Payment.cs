namespace PaymentService.Domain.Entities;

public class Payment
{
    public string Id { get; private set; }
    public string OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string PaymentMethod { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Payment(string orderId, decimal amount, string paymentMethod)
    {
        Id = Guid.NewGuid().ToString();
        OrderId = orderId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = "Pending";
        CreatedAt = DateTime.UtcNow;
    }
}