namespace ProductService.Application.DTOs;

public record ProductDto(string Name, decimal Price, int Stock, string Category);