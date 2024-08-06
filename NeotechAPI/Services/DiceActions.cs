using Neotech.DiceModels;

namespace Neotech.Services;

public static class DiceActions
{
    public static DiceSet StandardRoll(RollType rollType = RollType.Basic, uint extraDice = 0)
    {
        switch (rollType)
        {
            case RollType.Basic when extraDice is not 0:
                throw new ArgumentException($"No extra dice are allowed for {rollType}.");

            case RollType.Auto or RollType.Flow when extraDice is 0:
                throw new ArgumentException($"At least one extra die is required for {rollType}.");       
        }

       return new DiceSet(2 + extraDice);
    }
}


public static class DiceSetActions
{
    public static DiceSet Explode(this DiceSet diceSet, bool doubleChance = false)
    {
        var explosionCount = diceSet.Dice.Where(die => die.Result == 10 || (doubleChance && die.Result == 9)).Count();
        var explosions = new List<Die>();

        for (var iteration = 1; iteration <= explosionCount; iteration++)
        {
            var explosion = new Die();
            if (explosion.Result == 10 || (doubleChance && explosion.Result == 9))
            {
                explosionCount++;
            }
            explosions.Add(explosion);
        }
        return new DiceSet(explosions.ToArray());
    }

    public static DiceSet HighestTwo(this DiceSet diceSet)
    {
        if (diceSet.Dice.Length < 2) throw new ArgumentOutOfRangeException("The HighestTwo() method requires a DiceSet with at least 2 dice.");

        var highestTwo = diceSet.Dice.OrderByDescending(die => die.Result).Take(2).ToArray();
        return new DiceSet(highestTwo);
    }

    public static DiceSet? HighestPairOrDefault(this DiceSet diceSet)
    {
        var highestPair = diceSet.Dice.GroupBy(die => die.Result)
                                      .OrderByDescending(group => group.Count())
                                      .TakeWhile(group => group.Count() > 1)
                                      .OrderByDescending(group => group.Key)
                                      .First()
                                      .Take(2)
                                      .ToArray();
        return highestPair == null ? null : new DiceSet(highestPair);
    }
}
