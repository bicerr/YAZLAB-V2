using OrderService.Domain.Entities;

namespace OrderService.Tests;

public class OrderTests
{
    [Fact]
    public void Order_ShouldHaveProductId_WhenCreated()
    {
        var order = new Order("product-1", 2, "test@test.com");
        Assert.Equal("product-1", order.ProductId);
    }

    [Fact]
    public void Order_ShouldHaveQuantity_WhenCreated()
    {
        var order = new Order("product-1", 2, "test@test.com");
        Assert.Equal(2, order.Quantity);
    }

    [Fact]
    public void Order_ShouldHaveDefaultStatus_WhenCreated()
    {
        var order = new Order("product-1", 2, "test@test.com");
        Assert.Equal("Beklemede", order.Status);
    }

    [Fact]
    public void Order_ShouldHaveCustomerEmail_WhenCreated()
    {
        var order = new Order("product-1", 2, "test@test.com");
        Assert.Equal("test@test.com", order.CustomerEmail);
    }
}