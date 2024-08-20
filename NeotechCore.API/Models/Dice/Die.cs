namespace Neotech.Models;

public class Die
{
    private static Random _random = new Random();
    public virtual DiceType DiceType { get; }
    public virtual int Result { get; }

    public Die()
    {
        DiceType = DiceType.d10;
        Result = _random.Next(1, (int)DiceType + 1);
    }
    public Die(DiceType diceType) : this() => DiceType = diceType;
}
