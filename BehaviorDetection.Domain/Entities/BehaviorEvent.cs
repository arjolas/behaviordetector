using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
namespace BehaviorDetection.Domain.Entities;
public class BehaviorEvent
{
    public Guid Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    [NotMapped]
    public Dictionary<string, string> Data { get; set; } = new();

    // 👇 Questo viene mappato nel DB
    public string DataJson
    {
        get => JsonSerializer.Serialize(Data);
        set => Data = string.IsNullOrWhiteSpace(value)
            ? new Dictionary<string, string>()
            : JsonSerializer.Deserialize<Dictionary<string, string>>(value)!;
    }
}
