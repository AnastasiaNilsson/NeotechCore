namespace Neotech.DiceModels;

public class DiceSet
{
    public Die[] Dice { get; }
    public DiceType DiceType { get; }

    public DiceSet(uint numberOfDice, DiceType diceType = DiceType.d10)
    {
        Dice = Enumerable.Range(1, (int)numberOfDice).Select(_ => new Die(diceType)).ToArray();
        DiceType = diceType;
    }

    public DiceSet(Die[] existingDice)
    {
        if (!existingDice.Any())
        {
            throw new ArgumentOutOfRangeException("A DiceSet must contain at least one die.");
        }
        var diceType = existingDice.First().DiceType;
        if (existingDice.Where(die => die.DiceType != diceType).Any())
        {
            throw new ArgumentException("All dice in a DiceSet must have the same DiceType");
        }

        DiceType = diceType;
        Dice = existingDice;
    }

    public static DiceSet operator + (DiceSet setOne, DiceSet setTwo)
    {
        return new DiceSet(setOne.Dice.Concat(setTwo.Dice).ToArray());
    }
}
