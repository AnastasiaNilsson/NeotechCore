using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class StandardRollResult
{
    [JsonPropertyName("base_dice")]
    required public List<RolledSingleDie> BaseDice { get; init; }

    [JsonPropertyName("explosion_dice")]
    required public List<RolledSingleDie> ExplosionDice { get; init; }

    [JsonPropertyName("dice_result")]
    public int DiceResult { get; init; }

    [JsonPropertyName("explosion_dice")]
    public int AttributeScore { get; init; }

    [JsonPropertyName("explosion_dice")]
    public int EdgeBonus { get; init; }

    [JsonPropertyName("explosion_dice")]
    public int Total { get; init; }

    [JsonPropertyName("explosion_dice")]
    public int Difficulty { get; init; }

    [JsonPropertyName("result")]
    public ResultType Result { get; init; }
}
