namespace Neotech.DiceModels;

public class FinalRoll
{
    public int[] Dice {get;}
    public bool BaseDiceAreEqual {get;}
    public FinalRoll(int[] dice, bool baseDiceAreEqual)
    {
        Dice = dice;
        BaseDiceAreEqual = baseDiceAreEqual;
    }

    public int HighestDie {get => Dice.Max();}
    public int LowestDie {get => Dice.Min();}
    public int Total {get => Dice.Sum();}
}
