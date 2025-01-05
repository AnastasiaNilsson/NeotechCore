using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class RollRequest()
{
    [JsonPropertyName("requested_options")]
    required public RollOptions Options { get; init; }

    [JsonPropertyName("requested_by_client")]
    required public string RequestedByClient { get; init; }
    
    public DateTime RequestedAt { get; } = DateTime.Now;
}
