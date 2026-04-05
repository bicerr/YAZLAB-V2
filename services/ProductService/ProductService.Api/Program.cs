using MongoDB.Driver;
using ProductService.Application.Repositories;
using ProductService.Application.Services;
using ProductService.Infrastructure.Repositories;
using ProductService.Infrastructure.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// MongoDB
var mongoClient = new MongoClient("mongodb://mongodb:27017");
var database = mongoClient.GetDatabase("product_db");
var productsCollection = database.GetCollection<ProductService.Domain.Entities.Product>("products");

// Dependency Injection
builder.Services.AddSingleton(productsCollection);
builder.Services.AddScoped<IProductRepository, MongoProductRepository>();
builder.Services.AddScoped<IProductService, ProductServiceImpl>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpMetrics();
app.UseAuthorization();
app.MapControllers();
app.MapMetrics();
app.Run();