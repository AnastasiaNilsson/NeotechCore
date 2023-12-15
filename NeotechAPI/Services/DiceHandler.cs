namespace Services.Dice;

public static class DiceHandler
{
    public static FinalRoll FinalizePrelinaryRoll(this PreliminaryRoll roll, int[]? selectedBaseDice = null, bool useExplodingDice = true)
    {
        var selectedDice = roll.UseAutoDice == true || selectedBaseDice == null ? 
                           roll.HighestTwo().ToList() :
                           selectedBaseDice.ToList();

        var baseDiceAreEqual = selectedDice[0] == selectedDice[1];
        if (useExplodingDice)
        {
            selectedDice.AddRange(roll.Explode());
        }

        return new FinalRoll(selectedDice.ToArray(), baseDiceAreEqual);
    }

    public static bool EqualBaseDicePassesSkillCheck(this PreliminaryRoll preliminaryRoll, int difficulty = 20)
    {
        return false;
    } 
}
