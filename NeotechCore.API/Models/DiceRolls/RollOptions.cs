using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

public class RollOptions
{
    [JsonPropertyName("roll_type")]
    public RollType RollType { get; }

    [JsonPropertyName("number_of_dice")]
    public uint NumberOfDice { get; }

    [JsonPropertyName("attribute_score")]
    public uint AttributeScore { get; }

    [JsonPropertyName("edge_bonus")]
    public uint EdgeBonus { get; }

    [JsonPropertyName("difficulty")]
    public uint Difficulty { get; }

    [JsonPropertyName("joss")]
    public bool Joss { get; }

    public RollOptions()
    {
        RollType = RollType.Basic;
        NumberOfDice = 0;
        AttributeScore = 0;
        EdgeBonus = 0;
        Difficulty = 20;
        Joss = false;
    }
}
