using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using BehaviorDetection.Domain.Entities;
using BehaviorDetection.Domain.Interfaces;


namespace BehaviorDetection.Agents.Agents;

public class CollectorAgent : BackgroundService
{
    private readonly ChannelReader<BehaviorEvent> _channel;
    private readonly IEventRepository _repository;

    public CollectorAgent(ChannelReader<BehaviorEvent> channel, IEventRepository repository)
    {
        _channel = channel;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var behaviorEvent in _channel.ReadAllAsync(stoppingToken))
        {
            await _repository.SaveEventAsync(behaviorEvent);
        }
    }
}
