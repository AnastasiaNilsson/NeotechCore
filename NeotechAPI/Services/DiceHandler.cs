
namespace Services.Dice;

public static class DiceHandler
{
    public static Roll FinalizePrelinaryRoll(this PreliminaryRoll preliminaryRoll, (int, int)? selectedBaseDiceIndex = null, bool explodingDice = true)
    {
        var baseDice = (preliminaryRoll.UseAutoDice == true || selectedBaseDiceIndex == null) ? preliminaryRoll.HighestBaseDice() :
                       (preliminaryRoll.Dice[selectedBaseDiceIndex.Value.Item1], preliminaryRoll.Dice[selectedBaseDiceIndex.Value.Item2]);

        var baseDiceAreEqual = baseDice.Item1[0] == baseDice.Item2[0];
        var returnDice = new List<int>();

        switch (explodingDice)
        {
            case true:
                returnDice.AddRange(baseDice.Item1);
                returnDice.AddRange(baseDice.Item2);
                break;
            case false:
                returnDice.Add(baseDice.Item1[0]);
                returnDice.Add(baseDice.Item2[0]);
                break;
        }  

        return new Roll(returnDice, baseDiceAreEqual);
    }

    public static bool EqualBaseDicePassesSkillCheck(this PreliminaryRoll preliminaryRoll, int difficulty)
    {
        return false;
    }

}
