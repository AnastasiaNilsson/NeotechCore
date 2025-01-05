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

    public static List<object[]> GenerateManyRollOptions(int number)
    {
        var possibleRollTypes = Enum.GetValues(typeof(RollType));
        var possibleEdgeBonus = new int[] { 0, 2, 4, 6 };
        var random = new Random();

        return Enumerable.Range(1, number).Select(_ =>
        {
            var rollType = (RollType)possibleRollTypes.GetValue(random.Next(possibleRollTypes.Length))!;
            var extraDice = rollType == RollType.Basic ? 0 : random.Next(4) + 1;
            var attributeScore = random.Next(12);
            var edgeBonus = (int)possibleEdgeBonus.GetValue(random.Next(possibleEdgeBonus.Length))!;
            var difficulty = 15 + random.Next(15);
            var joss = Convert.ToBoolean(random.Next(2));

            return new object[] { new RollOptions()
            {
                RollType = rollType,
                ExtraDice = (uint)extraDice,
                AttributeScore = (uint)attributeScore,
                EdgeBonus = (uint)edgeBonus,
                Difficulty = (uint)difficulty,
                Joss = joss
            }};
        }).ToList();
    }
}
