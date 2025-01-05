using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class StandardRollResult
{
    [JsonPropertyName("result")]
    public ResultType Result { get; init; }

    [JsonPropertyName("difficulty")]
    public int Difficulty { get; init; }

    [JsonPropertyName("total_result")]
    public int TotalResult { get; init; }

    [JsonPropertyName("attribute_score")]
    public int AttributeScore { get; init; }

    [JsonPropertyName("edge_bonus")]
    public int EdgeBonus { get; init; }

    [JsonPropertyName("dice_result")]
    public int DiceResult { get; init; }

    [JsonPropertyName("base_dice")]
    required public List<int> BaseDice { get; init; }

    [JsonPropertyName("explosion_dice")]
    required public List<int> ExplosionDice { get; init; }
}
