namespace NeotechCore.Tests.UnitTests;

public static class TestHelper
{
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