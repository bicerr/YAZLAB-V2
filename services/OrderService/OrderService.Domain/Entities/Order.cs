namespace OrderService.Domain.Entities;

public class Order
{
    public string Id { get; private set; }
    public string ProductId { get; private set; }
    public int Quantity { get; private set; }
    public string CustomerEmail { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Order(string productId, int quantity, string customerEmail)
    {
        Id = Guid.NewGuid().ToString();
        ProductId = productId;
        Quantity = quantity;
        CustomerEmail = customerEmail;
        Status = "Beklemede";
        CreatedAt = DateTime.UtcNow;
    }
}