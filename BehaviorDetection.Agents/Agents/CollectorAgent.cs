using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BehaviorDetection.Domain.Entities;
using BehaviorDetection.Domain.Interfaces;
using Microsoft.Extensions.Logging;
namespace BehaviorDetection.Agents.Agents;

public class CollectorAgent : BackgroundService
{
    private readonly ChannelReader<BehaviorEvent> _channel;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CollectorAgent> _logger;

    public CollectorAgent(ChannelReader<BehaviorEvent> channel, IServiceScopeFactory scopeFactory, ILogger<CollectorAgent> logger)
    {
        _channel = channel;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var behaviorEvent in _channel.ReadAllAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IEventRepository>();
            await repository.SaveEventAsync(behaviorEvent);
            _logger.LogInformation($"Evento salvato: {behaviorEvent.EventType} @ {behaviorEvent.Timestamp}");
        }
    }
}
