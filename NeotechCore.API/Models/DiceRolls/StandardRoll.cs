using NeotechCore.API.Exceptions;

namespace NeotechCore.API.Models;

public class StandardRoll
{
    public List<SingleRolledDie> DicePool { get; init; }
    public List<SingleRolledDie> SelectedBaseDice { get; init; }
    public List<SingleRolledDie> Explosions { get; init; } = [];
    public RollOptions Options { get; init; } = new();

    public StandardRoll()
    {
        DicePool = Enumerable.Range(1, 2).Select(die => new SingleRolledDie()).ToList();
        SelectedBaseDice = DicePool;
    }

    public StandardRoll(List<SingleRolledDie> diceList) : this()
    {
        var firstDie = diceList.FirstOrDefault();
        if (firstDie is null) throw RolledDiceException.EmptyDiceList;
        if (diceList.Exists(die => die.DiceType != firstDie.DiceType)) throw RolledDiceException.MultipleDiceTypes;

        DicePool = diceList;
    }
    public StandardRoll(List<SingleRolledDie> diceList, RollOptions options) : this(diceList) => Options = options;

    public static StandardRoll operator +(StandardRoll setOne, StandardRoll setTwo)
    {
        var options = setOne.Options;
        var dice = setOne.DicePool.Concat(setTwo.DicePool).ToList();
        return new StandardRoll(dice, options);
    }
}
