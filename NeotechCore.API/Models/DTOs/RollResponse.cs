using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class RollResponse()
{
    [JsonPropertyName("roll_results")]
    required public StandardRollResult StandardRollResults { get; init; }

    [JsonPropertyName("requested_options")]
    required public RollOptions Options { get; init; }

    [JsonPropertyName("requested_by_client")]
    required public string RequestedByClient { get; init; }
    
    [JsonPropertyName("request_received")]
    public DateTime RequestReceived { get; init; }

    [JsonPropertyName("response_sent")]
    public DateTime ResponseSent { get; } = DateTime.Now;
}
