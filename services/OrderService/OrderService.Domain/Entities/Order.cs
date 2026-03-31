namespace OrderService.Domain.Entities;

public class Order
{
    public string Id { get; private set; }
    public string ProductId { get; private set; }
    public int Quantity { get; private set; }
    public string CustomerEmail { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Order(string productId, int quantity, string customerEmail)
    {
        ArgumentException.ThrowIfNullOrEmpty(productId);
        ArgumentException.ThrowIfNullOrEmpty(customerEmail);
        if (quantity <= 0) throw new ArgumentException("Miktar sıfırdan büyük olmalıdır.");

        Id = Guid.NewGuid().ToString();
        ProductId = productId;
        Quantity = quantity;
        CustomerEmail = customerEmail;
        Status = "Beklemede";
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(string status)
    {
        ArgumentException.ThrowIfNullOrEmpty(status);
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}