namespace Neotech.DiceModels;

public struct Die
{
    private static Random _random = new Random();

    public DiceType DiceType { get; }
    public int Result { get; }
    
    public Die(DiceType diceType)
    {
        DiceType = diceType;
        Result = _random.Next(1, (int)DiceType + 1);
    }
}
