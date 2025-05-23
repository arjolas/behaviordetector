using Microsoft.EntityFrameworkCore;
using BehaviorDetection.Infrastructure.Persistence;
using BehaviorDetection.Infrastructure.Repositories;
using BehaviorDetection.Domain.Interfaces;
using BehaviorDetection.Agents.Agents;
using BehaviorDetection.Domain.Entities;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// ✅ CONFIGURA EF CORE INMEMORY
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("BehaviorEvents"));

// ✅ REGISTRAZIONE REPOSITORY
builder.Services.AddScoped<IEventRepository, EventRepository>();

// ✅ CONFIGURA CHANNEL COMUNICAZIONE EVENTI
var behaviorEventChannel = Channel.CreateUnbounded<BehaviorEvent>();
builder.Services.AddSingleton(behaviorEventChannel);
builder.Services.AddSingleton<InMemoryRateLimiter>();
builder.Services.AddSingleton<ChannelReader<BehaviorEvent>>(provider => behaviorEventChannel.Reader);
builder.Services.AddSingleton<ChannelWriter<BehaviorEvent>>(provider => behaviorEventChannel.Writer);

// ✅ REGISTRAZIONE AGENTE COLLECTOR
builder.Services.AddHostedService<CollectorAgent>();
builder.Services.AddHostedService<AnomalyDetectionAgent>();

// ✅ API + SWAGGER
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ MIDDLEWARE
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
