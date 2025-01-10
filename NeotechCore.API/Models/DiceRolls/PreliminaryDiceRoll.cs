using NeotechCore.API.Exceptions;

namespace NeotechCore.API.Models;

public class PreliminaryDiceRoll : IDiceRoll
{
    public List<SingleRolledDie> DicePool { get; }
    public RollOptions Options { get; }
    public DiceType DiceType { get => DicePool.First().DiceType; }

    public PreliminaryDiceRoll(List<SingleRolledDie> diceList)
    {
        var firstDie = diceList.FirstOrDefault();
        if (firstDie is null) throw RolledDiceException.EmptyDiceList;
        if (diceList.Exists(die => die.DiceType != firstDie.DiceType)) throw RolledDiceException.MultipleDiceTypes;

        DicePool = diceList;
    }
    public PreliminaryDiceRoll(List<SingleRolledDie> diceList, RollOptions options) : this(diceList) => Options = options;

    public static PreliminaryDiceRoll operator +(PreliminaryDiceRoll setOne, PreliminaryDiceRoll setTwo)
    {
        var options = setOne.Options;
        var dice = setOne.DicePool.Concat(setTwo.DicePool).ToList();
        return new PreliminaryDiceRoll(dice, options);
    }
}
