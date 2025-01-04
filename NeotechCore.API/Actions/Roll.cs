using NeotechCore.API.Exceptions;
using NeotechCore.API.ModelExtensions;
using NeotechCore.API.Models;

namespace NeotechCore.API.Actions;

public static class Roll
{
    private static Random _random = new Random();

    public static RolledSingleDie SingleDie(DiceType diceType)
    {
        var result = _random.Next(1, (int)diceType + 1);
        return new RolledSingleDie(diceType, result);
    }

    public static List<RolledSingleDie> MultipleDice(int numberOfDice, DiceType diceType = DiceType.d10)
    {
        return Enumerable.Range(1, numberOfDice).Select(die => SingleDie(diceType)).ToList();
    }

    public static RolledDice Dice(uint numberOfDice, DiceType diceType)
    {
        var diceList = new List<RolledSingleDie>();
        foreach (var _ in Enumerable.Range(1, (int)numberOfDice))
        {
            diceList.Add(SingleDie(diceType));
        }
        return new RolledDice(diceList);
    }

    public static RolledDice Explosion(RolledDice rolledDice, bool doubleChance = false)
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
        return new RolledDice(explosions);
    }

    public static RolledDice StandardRoll(RollOptions options)
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
