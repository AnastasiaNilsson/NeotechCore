namespace Services.Dice;

public class PreliminaryRoll
{
    public bool UseAutoDice {get;}
    public List<List<int>> Dice {get => _dice;}
    private readonly List<List<int>> _dice;
      
    public PreliminaryRoll(List<List<int>> dice, bool useAutoDice)
    {
        dice.Sort((List<int> first, List<int> second) => first[0].CompareTo(second[0]));
        _dice = dice;
        UseAutoDice = useAutoDice;
    }

    public (List<int>, List<int>) HighestBaseDice()
    {  
        var returnDice = Dice.TakeLast(2).ToList();
        return (returnDice[0], returnDice[1]);
    }
    public (List<int>, List<int>)? HighestEqualBaseDice()
    {
        if (UseAutoDice == true) return null;
        var returnDice = Dice.GroupBy(dice => dice[0]).OrderDescending().FirstOrDefault(group => group.Count() >= 2)?.ToList();
        return returnDice == null ? null : (returnDice[0], returnDice[1]);
    }
}
