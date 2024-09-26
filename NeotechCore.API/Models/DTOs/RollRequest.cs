using System.Text.Json.Serialization;
using NeotechCore.API.Models;

namespace NeotechCore.API.Models;

public class DiceRequest()
{
    required public RollOptions Options { get; init; }
    required public string RequestedByClient { get; init; }
    public DateTime RequestedAt { get; } = DateTime.Now;
}
