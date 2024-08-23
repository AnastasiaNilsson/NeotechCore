using NeotechCore.API.Exceptions;

namespace NeotechCore.API.Models;

public class DiceSet
{
    public List<RolledDie> Dice { get; }
    public DiceType DiceType { get; }
    public RollModifiers Modifiers { get; }

    public DiceSet(DiceSet rolledDice, RollModifiers modifiers) : this(rolledDice.Dice) => Modifiers = modifiers;
    public DiceSet(List<RolledDie> rolledDice, RollModifiers modifiers) : this(rolledDice) => Modifiers = modifiers;
    public DiceSet(List<RolledDie> rolledDice)
    {
        var firstDie = rolledDice.FirstOrDefault();

        if (firstDie is null) throw DiceSetException.EmptyList;
        if (rolledDice.Exists(die => die.DiceType != firstDie.DiceType)) throw DiceSetException.MultipleDiceTypes;

        Dice = rolledDice;
        DiceType = firstDie.DiceType;
        Modifiers = new RollModifiers();
    }

    public static DiceSet operator +(DiceSet setOne, DiceSet setTwo)
    {
        var modifiers = setOne.Modifiers + setTwo.Modifiers;
        var dice = setOne.Dice.Concat(setTwo.Dice).ToList();
        return new DiceSet(dice, modifiers);
    }
}
