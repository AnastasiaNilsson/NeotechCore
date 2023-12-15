namespace Services.Dice;

public class Roll
{
    public int[] Dice {get;}
    public bool BaseDiceAreEqual {get;}
    public Roll(int[] dice, bool baseDiceAreEqual)
    {
        Dice = dice;
        BaseDiceAreEqual = baseDiceAreEqual;
    }

    public int HighestDie {get => Dice.Max();}
    public int LowestDie {get => Dice.Min();}
    public int Total {get => Dice.Sum();}
}
