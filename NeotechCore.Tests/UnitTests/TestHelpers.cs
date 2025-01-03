namespace NeotechCore.Tests.UnitTests;

public static class TestHelper
{
    public static List<RolledSingleDie> ManyDice(int numberOfDice, DiceType diceType = DiceType.d10)
    {
        var diceArray = new RolledSingleDie[numberOfDice];
        return diceArray.Select(die => Roll.SingleDie(diceType)).ToList();
    }

    public static List<RolledSingleDie> FakeDice(int[] results, DiceType diceType = DiceType.d10)
    {
        var diceCollection = new List<RolledSingleDie>();

        foreach (var result in results)
        {
            var fake = Mock.Of<RolledSingleDie>(die => die.Result == result && die.DiceType == diceType);
            diceCollection.Add(fake);
        }

        return diceCollection;
    }
}