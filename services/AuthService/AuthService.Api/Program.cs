using AuthService.Application.Repositories;
using AuthService.Application.Services;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// MongoDB
var mongoClient = new MongoClient("mongodb://mongodb:27017");
var database = mongoClient.GetDatabase("auth_db");
var usersCollection = database.GetCollection<AuthService.Domain.Entities.User>("users");

// Dependency Injection
builder.Services.AddSingleton(usersCollection);
builder.Services.AddScoped<IUserRepository, MongoUserRepository>();
builder.Services.AddScoped<IAuthService, JwtAuthService>();

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