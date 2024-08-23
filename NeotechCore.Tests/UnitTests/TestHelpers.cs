namespace NeotechCore.Tests.UnitTests;

public static class TestHelper
{
    public static List<RolledDie> ManyDice(int numberOfDice, DiceType diceType = DiceType.d10)
    {
        var diceArray = new RolledDie[numberOfDice];
        return diceArray.Select(die => Roll.SingleDie(diceType)).ToList();
    }

    public static List<RolledDie> FakeDice(int[] results, DiceType diceType = DiceType.d10)
    {
        var diceCollection = new List<RolledDie>();

        foreach (var result in results)
        {
            var fake = Mock.Of<RolledDie>(die => die.Result == result && die.DiceType == diceType);
            diceCollection.Add(fake);
        }

        return diceCollection;
    }
}