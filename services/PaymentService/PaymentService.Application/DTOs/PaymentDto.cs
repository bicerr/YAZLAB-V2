namespace PaymentService.Application.DTOs;

public record PaymentDto(string OrderId, decimal Amount, string PaymentMethod);