using Dispatcher.Application.Forwarding;
using Dispatcher.Application.Logging;
using Dispatcher.Application.Routing;
using Dispatcher.Infrastructure.Http;
using Dispatcher.Infrastructure.Logging;
using Dispatcher.Infrastructure.Routing;
using Dispatcher.Api.Middleware;
using MongoDB.Driver;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// MongoDB
var mongoClient = new MongoClient("mongodb://mongodb:27017");
var database = mongoClient.GetDatabase("dispatcher_db");
var logsCollection = database.GetCollection<Dispatcher.Domain.Logging.LogEntry>("logs");
var routesCollection = database.GetCollection<Dispatcher.Domain.Routing.RouteConfig>("routes");

// Dependency Injection
builder.Services.AddSingleton(logsCollection);
builder.Services.AddSingleton(routesCollection);
builder.Services.AddScoped<ILogRepository, MongoLogRepository>();
builder.Services.AddScoped<IRouteRepository, MongoRouteRepository>();
builder.Services.AddScoped<IRequestForwarder, HttpRequestForwarder>();
builder.Services.AddHttpClient<HttpRequestForwarder>();

// YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpMetrics();
app.UseMiddleware<AuthorizationMiddleware>();
app.MapControllers();
app.MapMetrics();
app.MapReverseProxy();
app.Run();