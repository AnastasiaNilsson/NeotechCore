
namespace NeotechCore.Tests.UnitTests;

public class RollTests
{

    [Fact]
    public void RolledSingleDie_DiceType_ShouldMatch_ParameterDiceType()
    {
        // Arrange & Act
        var die = Roll.SingleDie(DiceType.d100);

        // Assert
        die.DiceType.Should().Be(DiceType.d100);
    }

    [Fact]
    public void RolledSingleDie_Result_ShouldBeWithin_DiceTypeMaxValue()
    {
        // Arrange & Act
        var diceArray = Roll.MultipleDice(1000, DiceType.d10);

        // Assert
        foreach (var die in diceArray)
        {
            die.Result.Should().BeInRange(1, (int)DiceType.d10);
        }
    }

    [Theory]
    [MemberData(nameof(ExplosionTheory))]
    public void Explosion_ShouldAdd_CorrectNumberOfDice(RolledDice rolledDice, bool doubleChanceStatus)
    {
        // Arrange
        var originalDiceCount = rolledDice.DiceList.Count;

        // Act
        rolledDice += Roll.Explosion(rolledDice, doubleChanceStatus);

        var explosions = doubleChanceStatus ?
                         rolledDice.DiceList.Where(die => die.Result >= 9).ToArray() :
                         rolledDice.DiceList.Where(die => die.Result == 10).ToArray();

        var explosionCount = explosions.Length;
        var totalDiceCount = rolledDice.DiceList.Count;

        // Assert
        totalDiceCount.Should().Be(originalDiceCount + explosionCount);
    }
    public static List<object[]> ExplosionTheory()
    {
        var rolledDice1 = new RolledDice(TestHelper.FakeDice([10]));
        var rolledDice2 = new RolledDice(TestHelper.FakeDice([10, 1, 3, 8]));
        var rolledDice3 = new RolledDice(TestHelper.FakeDice([1, 9, 9, 2]));
        var rolledDice4 = new RolledDice(TestHelper.FakeDice([1, 10, 4, 10, 1]));

        bool doubleChance = true;
        bool noDoubleChance = false;

        return new List<object[]>()
        {
            {[rolledDice1, noDoubleChance]},
            {[rolledDice2, noDoubleChance]},
            {[rolledDice3, doubleChance]},
            {[rolledDice4, noDoubleChance]}
        };
    }

    [Fact]
    public void StandardRoll_ShouldThrow_IfParametersAreMismatched()
    {
        // Arrange & Act
        var rollOptions1 = new RollOptions() { RollType = RollType.Basic, NumberOfDice = 3 };
        var rollOptions2 = new RollOptions() { RollType = RollType.Auto, NumberOfDice = 2 };
        var rollOptions3 = new RollOptions() { RollType = RollType.Flow, NumberOfDice = 2 };

        // Act
        Action roll1 = () => Roll.StandardRoll(rollOptions1);
        Action roll2 = () => Roll.StandardRoll(rollOptions2);
        Action roll3 = () => Roll.StandardRoll(rollOptions3);

        // Assert
        roll1.Should().Throw<RollException>().WithMessage("No extra dice are allowed for RollType Basic.");
        roll2.Should().Throw<RollException>().WithMessage("At least one extra die is required for RollType Auto.");
        roll3.Should().Throw<RollException>().WithMessage("At least one extra die is required for RollType Flow.");
    }
}
