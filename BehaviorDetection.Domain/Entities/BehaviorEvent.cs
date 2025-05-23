using System;
using System.Collections.Generic;

namespace BehaviorDetection.Domain.Entities;

public class BehaviorEvent
{
    public Guid Id { get; set; }
    public string? SessionId { get; set; }
    public string? UserId { get; set; }
    public string? EventType { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, string> Data { get; set; } = new();
}
