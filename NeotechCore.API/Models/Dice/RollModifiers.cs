namespace NeotechCore.API.Models;

public class RollModifiers
{
    public uint RollBonus { get; }
    public uint Difficulty { get; }

    public RollModifiers(uint rollBonus = 0, uint difficulty = 20)
    {
        RollBonus = rollBonus;
        Difficulty = difficulty;
    }

    public static RollModifiers operator +(RollModifiers first, RollModifiers second)
    {
        var rollBonus =  first.RollBonus  != 0  ? first.RollBonus  : second.RollBonus;
        var difficulty = first.Difficulty != 20 ? first.Difficulty : second.Difficulty;

        return new RollModifiers(rollBonus, difficulty);
    }
}
