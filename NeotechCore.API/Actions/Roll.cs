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
        return new DiceSet(explosions, diceSet.Modifiers);
    }

    public static DiceSet StandardRoll(StandardRollType rollType = StandardRollType.Basic, uint extraDice = 0, uint rollBonus = 0, uint difficulty = 20)
    {
        switch (rollType)
        {
            case StandardRollType.Basic when extraDice is not 0:
                throw new ArgumentException($"No extra dice are allowed for {rollType}.");

            case StandardRollType.Auto or StandardRollType.Flow when extraDice is 0:
                throw new ArgumentException($"At least one extra die is required for {rollType}.");
        }

        var rolledDice = Roll.Dice(2 + extraDice, DiceType.d10)
                             .WithModifiers(rollBonus, difficulty);

        var baseDice = rollType == StandardRollType.Flow ?
                       rolledDice.BestToKeep() :
                       rolledDice.HighestTwo();

        return baseDice + Roll.Explosion(baseDice);
    }
}
