namespace Neotech.Models;

public class DiceSet
{
    public Die[] Dice { get; }
    public DiceType DiceType { get; }
    public uint RollBonus { get; }
    public uint Difficulty { get; }

    public DiceSet(uint numberOfDice, uint rollBonus = 0, uint difficulty = 20, DiceType diceType = DiceType.d10)
    {
        Dice = Enumerable.Range(1, (int)numberOfDice).Select(_ => new Die(diceType)).ToArray();
        DiceType = diceType;
        RollBonus = rollBonus;
        Difficulty = difficulty;
    }

    public DiceSet(Die[] existingDice, uint rollBonus = 0, uint difficulty = 20)
    {
        if (existingDice.Length == 0 || existingDice.All(value => value is null))
        {
            throw new ArgumentException("A DiceSet cannot be created with an empty array.");
        }

        var diceType = existingDice.First().DiceType;
        if (existingDice.Where(die => die.DiceType != diceType).Any())
        {
            throw new ArgumentException("All dice in a DiceSet must have the same DiceType.");
        }

        RollBonus = rollBonus;
        DiceType = diceType;
        Dice = existingDice.Where(value => value is not null).ToArray();
        Difficulty = difficulty;
    }

    public static DiceSet operator + (DiceSet setOne, DiceSet setTwo)
    {
        var rollBonus = setOne.RollBonus > 0 ? 
                        setOne.RollBonus : 
                        setTwo.RollBonus ;

        var difficulty = setOne.Difficulty != 20 || setTwo.Difficulty != 20 ?
                         Math.Max(setOne.Difficulty, setTwo.Difficulty) :
                         20;

        var dice = setOne.Dice.Concat(setTwo.Dice).ToArray();
        return new DiceSet(dice, rollBonus, difficulty);
    }
}
