namespace Services.Dice;

public class PreliminaryRoll
{
    public bool UseAutoDice {get;}
    public int[] Dice {get;}

    public PreliminaryRoll(int[] dice, bool useAutoDice = false)
    {
        if (dice.Length < 2) throw new ArgumentException("A valid roll cannot contain fewer than two dice.");
        Array.Sort(dice); 
        Dice = dice;
        UseAutoDice = useAutoDice;
    }

    public int[] HighestTwo() => Dice.TakeLast(2).ToArray();
    public int? HighestPairOrDefault()
    {
        if (UseAutoDice) return null;
        return Dice.GroupBy(die => die).Order().LastOrDefault(group => group.Count() > 1)?.Single();
    }
}
