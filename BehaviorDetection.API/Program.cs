using BehaviorDetection.Domain.Interfaces;
using BehaviorDetection.Infrastructure.Persistence;
using BehaviorDetection.Infrastructure.Repositories;
using BehaviorDetection.Agents.Agents;
using BehaviorDetection.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core (InMemory)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("BehaviorEvents"));

// Dependency Injection
builder.Services.AddScoped<IEventRepository, EventRepository>();

// Configure channel for communication between controller and agents
var channel = Channel.CreateUnbounded<BehaviorEvent>();
builder.Services.AddSingleton(channel);
builder.Services.AddHostedService<CollectorAgent>();

// Add controller support and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
