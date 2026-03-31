namespace ProductService.Domain.Entities;

public class Product
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string Category { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Product(string name, decimal price, int stock, string category)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(category);
        if (price < 0) throw new ArgumentException("Fiyat negatif olamaz.");
        if (stock < 0) throw new ArgumentException("Stok negatif olamaz.");

        Id = Guid.NewGuid().ToString();
        Name = name;
        Price = price;
        Stock = stock;
        Category = category;
        CreatedAt = DateTime.UtcNow;
    }
}