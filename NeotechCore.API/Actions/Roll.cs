using System.Diagnostics;
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

    public static StandardRollResult StandardRoll(RollOptions options)
    {
        switch (options.RollType)
        {
            case RollType.Basic when options.ExtraDice > 0:
                throw RollException.NoExtraDiceAllowed(options.RollType);

            case RollType.Auto or RollType.Flow when options.ExtraDice <= 0:
                throw RollException.ExtraDiceRequired(options.RollType);
        }

        var rolledDice = Roll.Dice(2 + options.ExtraDice, DiceType.d10)
                             .WithRollOptions(options);

        var baseDice = options.RollType == RollType.Flow ?
                       rolledDice.BestToKeep() :
                       rolledDice.HighestTwo();

        var explosions = rolledDice.Explosions(options.Joss);

        var diceResult = baseDice.DiceList.Concat(explosions)
                                          .Aggregate(0, (total, current) => total += current.Result);

        var totalResult = diceResult + (int)options.AttributeScore + (int)options.EdgeBonus;
        var baseDiceAreEqual = baseDice.DiceList[0].Result == baseDice.DiceList[1].Result;
        var difficulty = (int)options.Difficulty;

        var result = CalculateResult(baseDiceAreEqual, totalResult, difficulty);

        return new StandardRollResult()
        {
            BaseDice = baseDice.DiceList.Select(die => die.Result).ToList(),
            ExplosionDice = explosions.Select(die => die.Result).ToList(),
            DiceResult = diceResult,
            AttributeScore = (int)options.AttributeScore,
            EdgeBonus = (int)options.EdgeBonus,
            Total = totalResult,
            Difficulty = difficulty,
            Result = result
        };
    }

    private static ResultType CalculateResult(bool baseDiceAreEqual, int totalResult, int difficulty) => baseDiceAreEqual switch
    {
        true when totalResult >= difficulty => ResultType.InTheZone,
        false when totalResult >= difficulty => ResultType.Success,
        false when totalResult < difficulty => ResultType.Failure,
        true when totalResult < difficulty => ResultType.Fuckup
    };
}
