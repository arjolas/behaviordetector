using BehaviorDetection.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

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
}
