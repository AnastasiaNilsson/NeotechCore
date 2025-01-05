using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class RollRequest()
{
    [JsonPropertyName("roll_options")]
    required public RollOptions Options { get; init; }

    public DateTime RequestedAt { get; } = DateTime.Now;
}
