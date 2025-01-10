using NeotechCore.API.Models;
using NeotechCore.API.Exceptions;
using NeotechCore.API.Actions;

namespace NeotechCore.API.ModelExtensions;

public static class StandardRollExtension
{
    public static int RollBonus(this StandardRoll roll) => (int)roll.Options.AttributeScore + (int)roll.Options.EdgeBonus;
    public static int RollScore(this StandardRoll roll) => roll.SelectedBaseDice
                                                               .Concat(roll.Explosions)
                                                               .Aggregate(0, (total, die) => total += die.Result);
    public static int RollTotal(this StandardRoll roll) => roll.RollBonus() + roll.RollScore();

    public static StandardRoll ApplyRollOptions(this StandardRoll roll, RollOptions options)
    {
        var dicePool = roll.DicePool;
        if (options.ExtraDice > 0)
        {
            foreach (var _ in Enumerable.Range(1, (int)options.ExtraDice))
            {
                dicePool.Add(new SingleRolledDie());
            }
        }

        var baseDice = roll.BestToKeep(options);
        var explosions = roll.Explosions(options.Joss);

        return new StandardRoll()
        {
            DicePool = dicePool,
            SelectedBaseDice = baseDice,
            Explosions = explosions,
            Options = options
        };
    }

    private static StandardRoll AddRollOptions(this StandardRoll roll, RollOptions options) => new StandardRoll()
    {
            DicePool = roll.DicePool,
            SelectedBaseDice = roll.SelectedBaseDice,
            Explosions = roll.Explosions,
            Options = options
    };


    public static List<SingleRolledDie> HighestTwo(this StandardRoll roll)
    {
        if (roll.DicePool.Count < 2) throw RolledDiceException.HighestTwoRequiresTwo;

        var highestTwo = roll.DicePool.OrderByDescending(die => die.Result).Take(2).ToList();
        return new StandardRoll(highestTwo, roll.Options);
    }

    public static List<SingleRolledDie>? HighestPairOrDefault(this StandardRoll roll)
    {
        var highestPair = roll.DicePool.GroupBy(die => die.Result)
                                             .OrderByDescending(group => group.Count())
                                             .TakeWhile(group => group.Count() > 1)
                                             .OrderByDescending(group => group.Key)
                                             .FirstOrDefault()?
                                             .Take(2)
                                             .ToList();
        return highestPair == null ? null : new StandardRoll(highestPair, roll.Options);
    }

    public static List<SingleRolledDie> BestToKeep(this StandardRoll roll, RollOptions options)
    {
        var highestPair = roll.HighestPairOrDefault();
        var highestTwo = roll.HighestTwo();



        // Full implementation coming soon

        return roll.HighestTwo();
    }

    // Note to self: the explosions seem broken
    public static List<SingleRolledDie> Explosions(this StandardRoll roll, bool doubleChance = false)
    {
        var explosionCount = roll.DicePool.Where(die => die.Result == 10 || (doubleChance && die.Result == 9)).Count();
        var explosions = new List<SingleRolledDie>();

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
