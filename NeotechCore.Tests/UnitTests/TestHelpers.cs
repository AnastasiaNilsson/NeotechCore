namespace NeotechCore.Tests.UnitTests;

public static class TestHelper
{
    public static List<SingleRolledDie> ManyDice(int numberOfDice, DiceType diceType = DiceType.d10)
    {
        var diceArray = new SingleRolledDie[numberOfDice];
        return diceArray.Select(die => Roll.SingleDie(diceType)).ToList();
    }

    public static List<SingleRolledDie> FakeDice(int[] results, DiceType diceType = DiceType.d10)
    {
        var diceCollection = new List<SingleRolledDie>();

        foreach (var result in results)
        {
            var fake = Mock.Of<SingleRolledDie>(die => die.Result == result && die.DiceType == diceType);
            diceCollection.Add(fake);
        }

        return diceCollection;
    }
}