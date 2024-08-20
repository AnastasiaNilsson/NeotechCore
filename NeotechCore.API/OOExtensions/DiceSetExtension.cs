using Neotech.Models;

namespace Neotech.Extensions;

public static class DiceSetExtension
{
    public static DiceSet Explosion(this DiceSet diceSet, bool doubleChance = false)
    {
        var explosionCount = diceSet.Dice.Where(die => die.Result == 10 || (doubleChance && die.Result == 9)).Count();
        var explosions = new List<Die>();

        for (var iteration = 1; iteration <= explosionCount; iteration++)
        {
            var explosion = new Die(DiceType.d10);
            if (explosion.Result == 10 || (doubleChance && explosion.Result == 9))
            {
                explosionCount++;
            }
            explosions.Add(explosion);
        }
        return new DiceSet(explosions.ToArray(), diceSet.RollBonus);
    }

    public static DiceSet HighestTwo(this DiceSet diceSet)
    {
        if (diceSet.Dice.Length < 2) throw new ArgumentException("The HighestTwo() method requires a DiceSet with at least 2 dice.");

        var highestTwo = diceSet.Dice.OrderByDescending(die => die.Result).Take(2).ToArray();
        return new DiceSet(highestTwo);
    }

    public static DiceSet? HighestPairOrDefault(this DiceSet diceSet)
    {
        var highestPair = diceSet.Dice.GroupBy(die => die.Result)
                                      .OrderByDescending(group => group.Count())
                                      .TakeWhile(group => group.Count() > 1)
                                      .OrderByDescending(group => group.Key)
                                      .FirstOrDefault()?
                                      .Take(2)
                                      .ToArray();
        return highestPair == null ? null : new DiceSet(highestPair, diceSet.RollBonus);
    }

    public static DiceSet BestToKeep(this DiceSet diceSet)
    {
        // Implementation coming soon
        return new DiceSet(1);
    }
}
