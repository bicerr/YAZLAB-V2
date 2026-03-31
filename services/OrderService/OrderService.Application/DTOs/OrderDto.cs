namespace OrderService.Application.DTOs;

public record OrderDto(string ProductId, int Quantity, string CustomerEmail);