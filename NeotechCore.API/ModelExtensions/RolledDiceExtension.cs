using NeotechCore.API.Models;
using NeotechCore.API.Exceptions;
using NeotechCore.API.Actions;

namespace NeotechCore.API.ModelExtensions;

public static class RolledDiceExtension
{
    public static RolledDice WithRollOptions(this RolledDice rolledDice, RollOptions rollOptions)
    {
        return new RolledDice(rolledDice.DiceList, rollOptions);
    }

    public static RolledDice HighestTwo(this RolledDice rolledDice)
    {
        if (rolledDice.DiceList.Count < 2) throw RolledDiceException.HighestTwoRequiresTwo;

        var highestTwo = rolledDice.DiceList.OrderByDescending(die => die.Result).Take(2).ToList();
        return new RolledDice(highestTwo, rolledDice.Options);
    }

    public static RolledDice? HighestPairOrDefault(this RolledDice rolledDice)
    {
        var highestPair = rolledDice.DiceList.GroupBy(die => die.Result)
                                             .OrderByDescending(group => group.Count())
                                             .TakeWhile(group => group.Count() > 1)
                                             .OrderByDescending(group => group.Key)
                                             .FirstOrDefault()?
                                             .Take(2)
                                             .ToList();
        return highestPair == null ? null : new RolledDice(highestPair, rolledDice.Options);
    }

    public static RolledDice BestToKeep(this RolledDice rolledDice)
    {
        // Full implementation coming soon
        return rolledDice.HighestTwo();
    }

    // Note to self: the explosions seem broken
    public static List<RolledSingleDie> Explosions(this RolledDice rolledDice, bool doubleChance = false)
    {
        var explosionCount = rolledDice.DiceList.Where(die => die.Result == 10 || (doubleChance && die.Result == 9)).Count();
        var explosions = new List<RolledSingleDie>();

        for (var iteration = 1; iteration <= explosionCount; iteration++)
        {
            var explosion = Roll.SingleDie(DiceType.d10);
            if (explosion.Result == 10 || (doubleChance && explosion.Result == 9))
            {
                explosionCount++;
            }
            explosions.Add(explosion);
        }
        return explosions;
    }
}
