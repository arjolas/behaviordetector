using System.Collections.Concurrent;

public class InMemoryRateLimiter
{
    private readonly ConcurrentDictionary<string, List<DateTime>> _sessionHits = new();
    private readonly TimeSpan _window = TimeSpan.FromSeconds(10);
    private readonly int _maxRequests = 5;

    public bool IsAllowed(string sessionId)
    {
        var now = DateTime.UtcNow;
        var list = _sessionHits.GetOrAdd(sessionId, _ => new List<DateTime>());

        lock (list)
        {
            list.RemoveAll(ts => (now - ts) > _window);
            if (list.Count >= _maxRequests)
                return false;

            list.Add(now);
            return true;
        }
    }
}
