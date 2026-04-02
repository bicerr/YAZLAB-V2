using MongoDB.Driver;
using PaymentService.Application.Repositories;
using PaymentService.Application.Services;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDB
var mongoClient = new MongoClient("mongodb://mongodb:27017");
var database = mongoClient.GetDatabase("payment_db");
var paymentsCollection = database.GetCollection<PaymentService.Domain.Entities.Payment>("payments");

// Dependency Injection
builder.Services.AddSingleton(paymentsCollection);
builder.Services.AddScoped<IPaymentRepository, MongoPaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentServiceImpl>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();