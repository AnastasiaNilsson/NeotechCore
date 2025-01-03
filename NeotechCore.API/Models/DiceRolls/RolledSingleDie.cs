namespace NeotechCore.API.Models;

public class RolledSingleDie
{
    public virtual DiceType DiceType { get; } = DiceType.d10;
    public virtual int Result { get; } = 0;

    public RolledSingleDie() { }
    public RolledSingleDie(DiceType diceType, int result)
    {
        DiceType = diceType;
        Result = result;
    }
}
