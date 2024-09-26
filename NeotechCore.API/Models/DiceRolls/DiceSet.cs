using NeotechCore.API.Exceptions;

namespace NeotechCore.API.Models;

public class DiceSet
{
    public DiceType DiceType { get => Dice.First().DiceType; }
    public List<RolledDie> Dice { get; }
    public RollOptions Options { get; }

    public DiceSet(DiceSet rolledDice, RollOptions modifiers) : this(rolledDice.Dice) => Options = modifiers;
    public DiceSet(List<RolledDie> rolledDice, RollOptions modifiers) : this(rolledDice) => Options = modifiers;
    public DiceSet(List<RolledDie> rolledDice)
    {
        var firstDie = rolledDice.FirstOrDefault();

        if (firstDie is null) throw DiceSetException.EmptyList;
        if (rolledDice.Exists(die => die.DiceType != firstDie.DiceType)) throw DiceSetException.MultipleDiceTypes;

        Dice = rolledDice;
        Options = new RollOptions();
    }

    public static DiceSet operator +(DiceSet setOne, DiceSet setTwo)
    {
        var modifiers = setOne.Options;
        var dice = setOne.Dice.Concat(setTwo.Dice).ToList();
        return new DiceSet(dice, modifiers);
    }
}
