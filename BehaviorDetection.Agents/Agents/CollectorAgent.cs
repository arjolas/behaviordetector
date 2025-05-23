using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BehaviorDetection.Domain.Entities;
using BehaviorDetection.Domain.Interfaces;

namespace BehaviorDetection.Agents.Agents;

public class CollectorAgent : BackgroundService
{
    private readonly ChannelReader<BehaviorEvent> _channel;
    private readonly IServiceScopeFactory _scopeFactory;

    public CollectorAgent(ChannelReader<BehaviorEvent> channel, IServiceScopeFactory scopeFactory)
    {
        _channel = channel;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var behaviorEvent in _channel.ReadAllAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IEventRepository>();
            await repository.SaveEventAsync(behaviorEvent);
        }
    }
}
