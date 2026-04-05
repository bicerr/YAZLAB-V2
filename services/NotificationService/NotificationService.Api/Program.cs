using MongoDB.Driver;
using Prometheus;
using NotificationService.Application.Repositories;
using NotificationService.Application.Services;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDB
var mongoClient = new MongoClient("mongodb://mongodb:27017");
var database = mongoClient.GetDatabase("notification_db");
var notificationsCollection = database.GetCollection<NotificationService.Domain.Entities.Notification>("notifications");

// Dependency Injection
builder.Services.AddSingleton(notificationsCollection);
builder.Services.AddScoped<INotificationRepository, MongoNotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationServiceImpl>();

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