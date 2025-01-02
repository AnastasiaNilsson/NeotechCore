using NeotechCore.API.Models;
using NeotechCore.API.Exceptions;

namespace NeotechCore.API.ModelExtensions;

public static class DiceSetExtension
{
    public static RolledDice WithRollOptions(this RolledDice diceSet, RollOptions modifiers)
    {
        return new RolledDice(diceSet, modifiers);
    }

    public static RolledDice HighestTwo(this RolledDice diceSet)
    {
        if (diceSet.Dice.Count < 2) throw DiceSetException.HighestTwoRequiresTwo;

        var highestTwo = diceSet.Dice.OrderByDescending(die => die.Result).Take(2).ToList();
        return new RolledDice(highestTwo, diceSet.Options);
    }

    public static RolledDice? HighestPairOrDefault(this RolledDice diceSet)
    {
        var highestPair = diceSet.Dice.GroupBy(die => die.Result)
                                      .OrderByDescending(group => group.Count())
                                      .TakeWhile(group => group.Count() > 1)
                                      .OrderByDescending(group => group.Key)
                                      .FirstOrDefault()?
                                      .Take(2)
                                      .ToList();
        return highestPair == null ? null : new RolledDice(highestPair, diceSet.Options);
    }

    public static RolledDice BestToKeep(this RolledDice diceSet)
    {
        // Implementation coming soon
        return null;
    }
}
