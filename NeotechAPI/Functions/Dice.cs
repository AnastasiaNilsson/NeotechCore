
public class Dice
{
    private Random _random;
    public Dice() => _random = new Random();

    public int RollOne(Die numberOfSides) => _random.Next(1, (int)numberOfSides);


}


public enum Die
{
    d10 = 11,
    d17 = 18,
    d100 = 101
}