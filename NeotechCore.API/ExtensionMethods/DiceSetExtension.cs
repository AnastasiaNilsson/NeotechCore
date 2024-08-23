using NeotechCore.API.Models;
using NeotechCore.API.Exceptions;

namespace NeotechCore.API.ExtensionMethods;

public static class DiceSetExtension
{
    public static DiceSet WithModifiers(this DiceSet diceSet, uint rollBonus, uint difficulty)
    {
        var modifiers = new RollModifiers(rollBonus, difficulty);
        return new DiceSet(diceSet, modifiers);
    }

    public static DiceSet WithModifiers(this DiceSet diceSet, RollModifiers modifiers)
    {
        return new DiceSet(diceSet, modifiers);
    }

    public static DiceSet HighestTwo(this DiceSet diceSet)
    {
        if (diceSet.Dice.Count < 2) throw DiceSetException.HighestTwoRequiresTwo;

        var highestTwo = diceSet.Dice.OrderByDescending(die => die.Result).Take(2).ToList();
        return new DiceSet(highestTwo, diceSet.Modifiers);
    }

    public static DiceSet? HighestPairOrDefault(this DiceSet diceSet)
    {
        var highestPair = diceSet.Dice.GroupBy(die => die.Result)
                                      .OrderByDescending(group => group.Count())
                                      .TakeWhile(group => group.Count() > 1)
                                      .OrderByDescending(group => group.Key)
                                      .FirstOrDefault()?
                                      .Take(2)
                                      .ToList();
        return highestPair == null ? null : new DiceSet(highestPair, diceSet.Modifiers);
    }

    public static DiceSet BestToKeep(this DiceSet diceSet)
    {
        // Implementation coming soon
        return null;
    }
}
