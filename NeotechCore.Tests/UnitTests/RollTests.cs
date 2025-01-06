
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;

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
    public void Explosion_ShouldAdd_CorrectNumberOfDice(PreliminaryDiceRoll rolledDice, bool doubleChanceStatus)
    {
        // Arrange
        var originalDiceCount = rolledDice.DicePool.Count;

        // Act
        var newDiceList = rolledDice.DicePool;
        newDiceList.AddRange(rolledDice.Explosions(doubleChanceStatus));

        var explosions = doubleChanceStatus ?
                         newDiceList.Where(die => die.Result >= 9).ToArray() :
                         newDiceList.Where(die => die.Result == 10).ToArray();

        var explosionCount = explosions.Length;
        var totalDiceCount = rolledDice.DicePool.Count;

        // Assert
        totalDiceCount.Should().Be(originalDiceCount + explosionCount);
    }
    public static List<object[]> ExplosionTheory()
    {
        var rolledDice1 = new StandardRoll(TestHelper.FakeDice([10]));
        var rolledDice2 = new StandardRoll(TestHelper.FakeDice([10, 1, 3, 8]));
        var rolledDice3 = new StandardRoll(TestHelper.FakeDice([1, 9, 9, 2]));
        var rolledDice4 = new StandardRoll(TestHelper.FakeDice([1, 10, 4, 10, 1]));

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
        var rollOptions1 = new RollOptions() { RollType = RollType.Basic, ExtraDice = 1 };
        var rollOptions2 = new RollOptions() { RollType = RollType.Auto, ExtraDice = 0 };
        var rollOptions3 = new RollOptions() { RollType = RollType.Flow, ExtraDice = 0 };

        // Act
        Action roll1 = () => Roll.StandardRoll(rollOptions1);
        Action roll2 = () => Roll.StandardRoll(rollOptions2);
        Action roll3 = () => Roll.StandardRoll(rollOptions3);

        // Assert
        roll1.Should().Throw<RollException>().WithMessage("No extra dice are allowed for RollType Basic.");
        roll2.Should().Throw<RollException>().WithMessage("At least one extra die is required for RollType Auto.");
        roll3.Should().Throw<RollException>().WithMessage("At least one extra die is required for RollType Flow.");
    }

    [Theory]
    [MemberData(nameof(StandardRollTheory))]
    public void StandardRoll_ShouldReturn_CorrectStandardRollResults(RollOptions rollOptions)
    {
        // Arrange & Act
        var roll = Roll.StandardRoll(rollOptions);

        // Assert
        roll.BaseDice.Count().Should().Be(2);
        roll.EdgeBonus.Should().Be((int)rollOptions.EdgeBonus);
        roll.Difficulty.Should().Be((int)rollOptions.Difficulty);
        roll.AttributeScore.Should().Be((int)rollOptions.AttributeScore);
        roll.TotalResult.Should().Be(roll.BaseDice.Aggregate(0, (total, die) => total += die.Result) + roll.ExplosionDice.Aggregate(0, (total, die) => total += die.Result) + roll.AttributeScore + roll.EdgeBonus);

        if (roll.ExplosionDice.Any())
        {
            roll.BaseDice.Should().Contain(die => die.Result == 10 || (die.Result == 9 && rollOptions.Joss == true));
        }

        switch (roll.Result)
        {
            case ResultType.InTheZone:
                roll.BaseDice[0].Result.Should().Be(roll.BaseDice[1].Result);
                roll.TotalResult.Should().BeGreaterThanOrEqualTo(roll.Difficulty);
                break;
            case ResultType.Fuckup:
                roll.BaseDice[0].Result.Should().Be(roll.BaseDice[1].Result);
                roll.TotalResult.Should().BeLessThan(roll.Difficulty);
                break;
            case ResultType.Failure:
                roll.BaseDice[0].Result.Should().NotBe(roll.BaseDice[1].Result);
                roll.TotalResult.Should().BeLessThan(roll.Difficulty);
                break;
            case ResultType.Success:
                roll.BaseDice[0].Result.Should().NotBe(roll.BaseDice[1].Result);
                roll.TotalResult.Should().BeGreaterThanOrEqualTo(roll.Difficulty);
                break;
        }

    }
    public static List<object[]> StandardRollTheory() => TestHelper.GenerateManyRollOptions(1000);
}
