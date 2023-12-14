namespace Services.Dice;

public class Roll
{
    public List<int> Dice {get;}
    public bool BaseDiceAreEqual {get;}
    public Roll(List<int> dice, bool baseDiceAreEqual) 
    {
        Dice = dice;
        BaseDiceAreEqual = baseDiceAreEqual;
    }

    public int HighestDie {get => Dice.Max();}
    public int LowestDie {get => Dice.Min();}
    public int Total {get => Dice.Sum();}
}
