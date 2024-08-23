namespace NeotechCore.API.Models;

public class RolledDie
{
    public virtual DiceType DiceType { get; } = DiceType.d10;
    public virtual int Result { get; } = 0;

    public RolledDie() {}
    public RolledDie(DiceType diceType, int result) 
    {
        DiceType = diceType;
        Result = result;
    } 
}
