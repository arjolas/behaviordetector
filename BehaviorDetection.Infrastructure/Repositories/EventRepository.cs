using BehaviorDetection.Domain.Entities;
using BehaviorDetection.Domain.Interfaces;
using BehaviorDetection.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BehaviorDetection.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveEventAsync(BehaviorEvent evt)
    {
        _context.BehaviorEvents.Add(evt);
        await _context.SaveChangesAsync();
    }

    public async Task<List<BehaviorEvent>> GetEventsBySessionAsync(string sessionId)
    {
        return await _context.BehaviorEvents
            .Where(e => e.SessionId == sessionId)
            .ToListAsync();
    }
}
