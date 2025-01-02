namespace NeotechCore.API.Models;

public class SingleRolledDie
{
    public virtual DiceType DiceType { get; } = DiceType.d10;
    public virtual int Result { get; } = 0;

    public SingleRolledDie() { }
    public SingleRolledDie(DiceType diceType, int result)
    {
        DiceType = diceType;
        Result = result;
    }
}
