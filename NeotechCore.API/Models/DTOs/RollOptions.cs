using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class RollOptions()
{
    [JsonPropertyName("roll_type")]
    public RollType RollType { get; init; } = RollType.Basic;

    [JsonPropertyName("extra_dice")]
    public uint ExtraDice { get; init; } = 0;

    [JsonPropertyName("attribute_score")]
    public uint AttributeScore { get; init; } = 0;

    [JsonPropertyName("edge_bonus")]
    public uint EdgeBonus { get; init; } = 0;

    [JsonPropertyName("difficulty")]
    public uint Difficulty { get; init; } = 20;

    [JsonPropertyName("joss")]
    public bool Joss { get; init; } = false;
}
