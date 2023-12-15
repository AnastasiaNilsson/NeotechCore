
namespace Services.Dice;

public class PreliminaryRoll
{
    public bool UseAutoDice {get;}
    public int[][] Dice {get;}

    public PreliminaryRoll(int[][] dice, bool useAutoDice)
    {
        if (dice.Length < 2 || dice[0].Length < 1) throw new ArgumentException("A valid roll cannot contain fewer than two sets of dice.");

        Array.Sort<int[]>(dice, (x,y) => Comparer<int>.Default.Compare(x[0], y[0])); 
        Dice = dice;
        UseAutoDice = useAutoDice;
    }
}
