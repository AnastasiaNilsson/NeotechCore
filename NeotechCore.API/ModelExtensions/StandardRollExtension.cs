using NeotechCore.API.Models;
using NeotechCore.API.Exceptions;
using NeotechCore.API.Actions;

namespace NeotechCore.API.ModelExtensions;

public static class StandardRollExtension
{
    public static int RollBonus(this StandardRoll roll) => (int)roll.Options.AttributeScore + (int)roll.Options.EdgeBonus;
    public static int RollScore(this StandardRoll roll) => roll.SelectedBaseDice.Concat(roll.Explosions).Aggregate(0, (total, die) => total += die.Result);
    public static int RollTotal(this StandardRoll roll) => roll.RollBonus() + roll.RollScore();

    public static StandardRoll ApplyRollOptions(this StandardRoll rolledDice, RollOptions rollOptions)
    {
        return new StandardRoll(rolledDice.DicePool, rollOptions);
    }

    public static StandardRoll HighestTwo(this StandardRoll rolledDice)
    {
        if (rolledDice.DicePool.Count < 2) throw RolledDiceException.HighestTwoRequiresTwo;

        var highestTwo = rolledDice.DicePool.OrderByDescending(die => die.Result).Take(2).ToList();
        return new StandardRoll(highestTwo, rolledDice.Options);
    }

    public static StandardRoll? HighestPairOrDefault(this StandardRoll rolledDice)
    {
        var highestPair = rolledDice.DicePool.GroupBy(die => die.Result)
                                             .OrderByDescending(group => group.Count())
                                             .TakeWhile(group => group.Count() > 1)
                                             .OrderByDescending(group => group.Key)
                                             .FirstOrDefault()?
                                             .Take(2)
                                             .ToList();
        return highestPair == null ? null : new StandardRoll(highestPair, rolledDice.Options);
    }

    public static StandardRoll BestToKeep(this StandardRoll rolledDice)
    {
        // Full implementation coming soon
        return rolledDice.HighestTwo();
    }

    // Note to self: the explosions seem broken
    public static List<RolledSingleDie> Explosions(this StandardRoll rolledDice, bool doubleChance = false)
    {
        var explosionCount = rolledDice.DicePool.Where(die => die.Result == 10 || (doubleChance && die.Result == 9)).Count();
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
