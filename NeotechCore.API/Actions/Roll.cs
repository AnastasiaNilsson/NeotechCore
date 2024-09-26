using NeotechCore.API.Exceptions;
using NeotechCore.API.ExtensionMethods;
using NeotechCore.API.Models;

namespace NeotechCore.API.Actions;

public static class Roll
{
    private static Random _random = new Random();

    public static RolledDie SingleDie(DiceType diceType)
    {
        var result = _random.Next(1, (int)diceType + 1);
        return new RolledDie(diceType, result);
    }

    public static DiceSet Dice(uint numberOfDice, DiceType diceType)
    {
        var rolledDice = new List<RolledDie>();
        foreach (var i in Enumerable.Range(1, (int)numberOfDice))
        {
            rolledDice.Add(SingleDie(diceType));
        }
        return new DiceSet(rolledDice);
    }

    public static DiceSet Explosion(DiceSet diceSet, bool doubleChance = false)
    {
        var explosionCount = diceSet.Dice.Where(die => die.Result == 10 || (doubleChance && die.Result == 9)).Count();
        var explosions = new List<RolledDie>();

        for (var iteration = 1; iteration <= explosionCount; iteration++)
        {
            var explosion = Roll.SingleDie(DiceType.d10);
            if (explosion.Result == 10 || (doubleChance && explosion.Result == 9))
            {
                explosionCount++;
            }
            explosions.Add(explosion);
        }
        return new DiceSet(explosions, diceSet.Options);
    }

    public static DiceSet StandardRoll(RollOptions options)
    {
        switch (options.RollType)
        {
            case RollType.Basic when options.NumberOfDice > 2:
                throw RollException.NoExtraDiceAllowed(options.RollType);

            case RollType.Auto or RollType.Flow when options.NumberOfDice == 2:
                throw RollException.ExtraDiceRequired(options.RollType);
        }

        var rolledDice = Roll.Dice(options.NumberOfDice, DiceType.d10)
                             .WithRollOptions(options);

        var baseDice = options.RollType == RollType.Flow ?
                       rolledDice.BestToKeep() :
                       rolledDice.HighestTwo();

        return baseDice + Roll.Explosion(baseDice);
    }
}
