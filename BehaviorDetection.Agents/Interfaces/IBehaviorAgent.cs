using BehaviorDetection.Domain.Entities;

namespace BehaviorDetection.Agents.Interfaces;

public interface IBehaviorAgent
{
    Task HandleEventAsync(BehaviorEvent evt);
}
