using ProductService.Domain.Entities;

namespace ProductService.Tests;

public class ProductTests
{
    [Fact]
    public void Product_ShouldHaveName_WhenCreated()
    {
        var product = new Product("Laptop", 15000, 50, "Elektronik");
        Assert.Equal("Laptop", product.Name);
    }

    [Fact]
    public void Product_ShouldHavePrice_WhenCreated()
    {
        var product = new Product("Laptop", 15000, 50, "Elektronik");
        Assert.Equal(15000, product.Price);
    }

    [Fact]
    public void Product_ShouldHaveStock_WhenCreated()
    {
        var product = new Product("Laptop", 15000, 50, "Elektronik");
        Assert.Equal(50, product.Stock);
    }

    [Fact]
    public void Product_ShouldHaveCategory_WhenCreated()
    {
        var product = new Product("Laptop", 15000, 50, "Elektronik");
        Assert.Equal("Elektronik", product.Category);
    }
}