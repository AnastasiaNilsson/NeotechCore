using NeotechCore.API.Exceptions;

namespace NeotechCore.API.Models;

public class RolledDice
{
    public DiceType DiceType { get => DiceList.First().DiceType; }
    public List<SingleRolledDie> DiceList { get; }
    public RollOptions Options { get; }

    public RolledDice(List<SingleRolledDie> diceList, RollOptions modifiers) : this(diceList) => Options = modifiers;
    public RolledDice(RolledDice rolledDice, RollOptions modifiers) : this(rolledDice.DiceList) => Options = modifiers;
    public RolledDice(List<SingleRolledDie> diceList)
    {
        var firstDie = diceList.FirstOrDefault();

        if (firstDie is null) throw DiceSetException.EmptyList;
        if (diceList.Exists(die => die.DiceType != firstDie.DiceType)) throw DiceSetException.MultipleDiceTypes;

        DiceList = diceList;
        Options = new RollOptions();
    }

    public static RolledDice operator +(RolledDice setOne, RolledDice setTwo)
    {
        var modifiers = setOne.Options;
        var dice = setOne.DiceList.Concat(setTwo.DiceList).ToList();
        return new RolledDice(dice, modifiers);
    }
}
