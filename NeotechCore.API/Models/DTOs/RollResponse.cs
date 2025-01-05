using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class RollResponse()
{
    [JsonPropertyName("roll_results")]
    required public StandardRollResult StandardRollResults { get; init; }

    [JsonPropertyName("requested_options")]
    required public RollOptions RequestedOptions { get; init; }

    [JsonPropertyName("request_received")]
    required public DateTime RequestedAt { get; init; }

    [JsonPropertyName("response_sent")]
    public DateTime ResponseSent { get; } = DateTime.Now;
}
