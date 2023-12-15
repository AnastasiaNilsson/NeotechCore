
namespace Services.Dice;

// Refactor: Look at implementing a RolledDie class instead of these nested arrays
public static class RollHandler
{
    public static Roll FinalizePrelinaryRoll(this PreliminaryRoll preliminaryRoll, (int, int)? selectedBaseDiceIndex = null, bool explodingDice = true)
    {
        var baseDice = preliminaryRoll.UseAutoDice == true || selectedBaseDiceIndex == null ? 
                       preliminaryRoll.HighestBaseDice() :
                       (preliminaryRoll.Dice[selectedBaseDiceIndex.Value.Item1], preliminaryRoll.Dice[selectedBaseDiceIndex.Value.Item2]);

        var baseDiceAreEqual = baseDice.Item1[0] == baseDice.Item2[0];
        var finalDice = new List<int>();

        switch (explodingDice)
        {
            case true:
                finalDice.AddRange(baseDice.Item1);
                finalDice.AddRange(baseDice.Item2);
                break;
            case false:
                finalDice.Add(baseDice.Item1[0]);
                finalDice.Add(baseDice.Item2[0]);
                break;
        }  

        return new Roll(finalDice.ToArray(), baseDiceAreEqual);
    }

    public static bool EqualBaseDicePassesSkillCheck(this PreliminaryRoll preliminaryRoll, int difficulty = 20)
    {
        return false;
    }

    public static (int[], int[]) HighestBaseDice(this PreliminaryRoll preliminaryRoll)
    {  
        var returnDice = preliminaryRoll.Dice.TakeLast(2).ToArray();
        return (returnDice[0], returnDice[1]);
    }
    public static (int[], int[])? HighestEqualBaseDiceOrDefault(this PreliminaryRoll preliminaryRoll)
    {
        if (preliminaryRoll.UseAutoDice == true) return null;
        var returnDice = preliminaryRoll.Dice.GroupBy(dice => dice[0]).OrderDescending().FirstOrDefault(group => group.Count() >= 2)?.ToList();
        return returnDice == null ? null : (returnDice[0], returnDice[1]);
    }
}
