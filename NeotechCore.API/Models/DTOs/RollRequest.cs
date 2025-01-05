using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class RollRequest()
{
    [JsonPropertyName("options")]
    required public RollOptions Options { get; init; }

    public DateTime RequestedAt { get; } = DateTime.Now;
}
