using PaymentService.Domain.Entities;

namespace PaymentService.Tests;

public class PaymentTests
{
    [Fact]
    public void Payment_ShouldHaveOrderId_WhenCreated()
    {
        var payment = new Payment("order-1", 150.00m, "CreditCard");
        Assert.Equal("order-1", payment.OrderId);
    }

    [Fact]
    public void Payment_ShouldHaveAmount_WhenCreated()
    {
        var payment = new Payment("order-1", 150.00m, "CreditCard");
        Assert.Equal(150.00m, payment.Amount);
    }

    [Fact]
    public void Payment_ShouldHavePendingStatus_WhenCreated()
    {
        var payment = new Payment("order-1", 150.00m, "CreditCard");
        Assert.Equal("Pending", payment.Status);
    }

    [Fact]
    public void Payment_ShouldHavePaymentMethod_WhenCreated()
    {
        var payment = new Payment("order-1", 150.00m, "CreditCard");
        Assert.Equal("CreditCard", payment.PaymentMethod);
    }
}