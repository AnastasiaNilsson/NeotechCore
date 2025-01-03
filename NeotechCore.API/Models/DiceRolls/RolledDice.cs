using NeotechCore.API.Exceptions;

namespace NeotechCore.API.Models;

public class RolledDice
{
    public DiceType DiceType { get => DiceList.First().DiceType; }
    public List<RolledSingleDie> DiceList { get; }
    public RollOptions Options { get; }

    public RolledDice(List<RolledSingleDie> diceList)
    {
        var firstDie = diceList.FirstOrDefault();

        if (firstDie is null) throw RolledDiceException.EmptyDiceList;
        if (diceList.Exists(die => die.DiceType != firstDie.DiceType)) throw RolledDiceException.MultipleDiceTypes;

        DiceList = diceList;
        Options = new RollOptions();
    }
    public RolledDice(List<RolledSingleDie> diceList, RollOptions options) : this(diceList) => Options = options;

    public static RolledDice operator +(RolledDice setOne, RolledDice setTwo)
    {
        var options = setOne.Options;
        var dice = setOne.DiceList.Concat(setTwo.DiceList).ToList();
        return new RolledDice(dice, options);
    }
}
