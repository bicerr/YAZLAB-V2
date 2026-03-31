namespace ProductService.Domain.Entities;

public class Product
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string Category { get; private set; }

    public Product(string name, decimal price, int stock, string category)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Price = price;
        Stock = stock;
        Category = category;
    }
}