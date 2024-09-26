using NeotechCore.API.Exceptions;

namespace NeotechCore.Tests.UnitTests;

public class RollTests
{

    [Fact]
    public void SingleDie_DiceType_ShouldMatch_ParameterDiceType()
    {
        // Arrange & Act
        var die = Roll.SingleDie(DiceType.d100);

        // Assert
        die.DiceType.Should().Be(DiceType.d100);
    }

    [Fact]
    public void SingleDie_Result_ShouldBeWithin_DiceTypeMaxValue()
    {
        // Arrange & Act
        var diceArray = TestHelper.ManyDice(1000, DiceType.d10);

        // Assert
        foreach (var die in diceArray)
        {
            die.Result.Should().BeInRange(1, (int)DiceType.d10);
        }
    }

    [Theory]
    [MemberData(nameof(ExplosionTheory))]
    public void Explosion_ShouldAdd_CorrectNumberOfDice(DiceSet diceSet, bool doubleChanceStatus)
    {
        // Arrange
        var originalDiceCount = diceSet.Dice.Count;

        // Act
        diceSet += Roll.Explosion(diceSet, doubleChanceStatus);

        var explosions = doubleChanceStatus ?
                         diceSet.Dice.Where(die => die.Result >= 9).ToArray() :
                         diceSet.Dice.Where(die => die.Result == 10).ToArray();

        var explosionCount = explosions.Length;
        var totalDiceCount = diceSet.Dice.Count;

        // Assert
        totalDiceCount.Should().Be(originalDiceCount + explosionCount);
    }
    public static List<object[]> ExplosionTheory()
    {
        var diceSet1 = new DiceSet(TestHelper.FakeDice([10]));
        var diceSet2 = new DiceSet(TestHelper.FakeDice([10, 1, 3, 8]));
        var diceSet3 = new DiceSet(TestHelper.FakeDice([1, 9, 9, 2]));
        var diceSet4 = new DiceSet(TestHelper.FakeDice([1, 10, 4, 10, 1]));

        bool doubleChance = true;
        bool noDoubleChance = false;

        return new List<object[]>()
        {
            {[diceSet1, noDoubleChance]},
            {[diceSet2, noDoubleChance]},
            {[diceSet3, doubleChance]},
            {[diceSet4, noDoubleChance]}
        };
    }

    [Fact]
    public void StandardRoll_ShouldThrow_IfParametersAreMismatched()
    {
        // Arrange & Act
        Action roll1 = () => Roll.StandardRoll(API.Models.RollType.Basic, extraDice: 1);
        Action roll2 = () => Roll.StandardRoll(API.Models.RollType.Auto, extraDice: 0);
        Action roll3 = () => Roll.StandardRoll(API.Models.RollType.Flow, extraDice: 0);

        // Assert
        roll1.Should().Throw<RollException>().WithMessage("No extra dice are allowed for 'StandardRollType.Basic'.");
        roll2.Should().Throw<RollException>().WithMessage("At least one extra die is required for 'StandardRollType.Auto'.");
        roll3.Should().Throw<RollException>().WithMessage("At least one extra die is required for 'StandardRollType.Flow'.");
    }
}
