using BehaviorDetection.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using BehaviorDetection.Domain.Interfaces;

namespace BehaviorDetection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BehaviorController : ControllerBase
{
    private readonly ChannelWriter<BehaviorEvent> _writer;
    private readonly InMemoryRateLimiter _rateLimiter;

    public BehaviorController(Channel<BehaviorEvent> channel, InMemoryRateLimiter rateLimiter)
    {
        _writer = channel.Writer;
        _rateLimiter = rateLimiter;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BehaviorEvent evt)
    {
        if (!EventType.ValidTypes.Contains(evt.EventType.ToLowerInvariant()))
            return BadRequest($"Invalid event type: '{evt.EventType}'");

        if (!_rateLimiter.IsAllowed(evt.SessionId))
            return StatusCode(429, "Too many events from this session. Try again later.");

        await _writer.WriteAsync(evt);
        return Accepted();
    }


     // ✅ AGGIUNTO: recupero eventi per SessionId
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string sessionId, [FromServices] IEventRepository repository)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
            return BadRequest("Missing sessionId");

        var events = await repository.GetEventsBySessionAsync(sessionId);
        return Ok(events);
    }
}
