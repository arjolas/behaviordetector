using BehaviorDetection.Domain.Entities;
namespace BehaviorDetection.Domain.Interfaces;

public interface IEventRepository
{
    Task SaveEventAsync(BehaviorEvent evt);
    Task<List<BehaviorEvent>> GetEventsBySessionAsync(string sessionId);
}
