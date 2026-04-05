using Microsoft.AspNetCore.Mvc;
using ProductService.Application.DTOs;
using ProductService.Application.Services;
using Prometheus;

namespace ProductService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly Counter ProductsListedCounter = Metrics.CreateCounter(
        "products_listed_total", "Ürün listeleme istekleri");
    private static readonly Counter ProductsCreatedCounter = Metrics.CreateCounter(
        "products_created_total", "Ürün oluşturma istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });
    private static readonly Counter ProductsUpdatedCounter = Metrics.CreateCounter(
        "products_updated_total", "Ürün güncelleme istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });
    private static readonly Counter ProductsDeletedCounter = Metrics.CreateCounter(
        "products_deleted_total", "Ürün silme istekleri", new CounterConfiguration { LabelNames = new[] { "result" } });

    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        ProductsListedCounter.Inc();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound("Ürün bulunamadı.");
        return Ok(product);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        var products = await _productService.GetByCategoryAsync(category);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
        var result = await _productService.CreateAsync(dto);
        if (!result.Success)
        {
            ProductsCreatedCounter.WithLabels("failure").Inc();
            return BadRequest(result.ErrorMessage);
        }
        ProductsCreatedCounter.WithLabels("success").Inc();
        return Created("", result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductDto dto)
    {
        var result = await _productService.UpdateAsync(id, dto);
        if (!result.Success)
        {
            ProductsUpdatedCounter.WithLabels("failure").Inc();
            return NotFound(result.ErrorMessage);
        }
        ProductsUpdatedCounter.WithLabels("success").Inc();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result)
        {
            ProductsDeletedCounter.WithLabels("failure").Inc();
            return NotFound("Ürün bulunamadı.");
        }
        ProductsDeletedCounter.WithLabels("success").Inc();
        return Ok("Ürün silindi.");
    }
}