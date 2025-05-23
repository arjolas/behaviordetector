using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BehaviorDetection.Domain.Entities;
using BehaviorDetection.Domain.Interfaces;
using Microsoft.Extensions.Logging;
namespace BehaviorDetection.Agents.Agents;
public class AnomalyDetectionAgent : BackgroundService
{
    private readonly ChannelReader<BehaviorEvent> _channel;
    private readonly ILogger<AnomalyDetectionAgent> _logger;

    public AnomalyDetectionAgent(ChannelReader<BehaviorEvent> channel, ILogger<AnomalyDetectionAgent> logger)
    {
        _channel = channel;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var evt in _channel.ReadAllAsync(stoppingToken))
        {
            if (evt.EventType == "click" && evt.Data.ContainsKey("x"))
            {
                _logger.LogWarning($"Possibile anomalia rilevata: {evt.UserId} ha cliccato in {evt.Data["x"]},{evt.Data["y"]}");
            }
        }
    }
}
