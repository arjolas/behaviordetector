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

    public BehaviorController(Channel<BehaviorEvent> channel)
    {
        _writer = channel.Writer;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BehaviorEvent evt)
    {
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
