using FluentAssertions;
using Services.Dice;

namespace NeotechAPI.Tests;

public class DiceRollerTests
{
    public DiceRoller _diceService = new DiceRoller();

    [Fact]
    public void RollOneDie_ShouldReturn_IntBetween1and10()
    {
        // Arrange
        var resultList = new List<int>();

        // Act
        foreach (var _ in Enumerable.Range(1,100))
        {
            resultList.Add(_diceService.RollOne(Die.d10));
        }

        // Assert
        resultList.Should().OnlyContain(x => (1 <= x) && (x <= 10));
    }

    [Fact]
    public void RollDice_ShouldReturn_PreliminaryRoll()
    {
        // Arrange

        // Act
        var result = _diceService.RollDice();

        // Assert
        result.Should().BeOfType<PreliminaryRoll>();
        result.Dice.Count.Should().Be(2);
        result.UseAutoDice.Should().BeFalse();
    }

    [Fact]
    public void RollDice_WithParameters_ShouldReturn_PreliminaryRoll_WithCorrectProperties()
    {
        // Arrange
        var extraDice = 5;
        // Act
        var result = _diceService.RollDice(extraDice, true);

        // Assert
        result.Should().BeOfType<PreliminaryRoll>();
        result.Dice.Count.Should().Be(7);
        result.UseAutoDice.Should().BeTrue();
    }

    [Fact]
    public void CallingFinalize_ShouldReturn_FinalRoll()
    {
        // Arrange
        var rolls = new List<List<int>>() {};
        var preliminaryRoll = new PreliminaryRoll(rolls, false);

        // Act
        var result = preliminaryRoll.FinalizePrelinaryRoll();

        // Assert
        result.Should().BeOfType<Roll>();
    }

    [Fact]
    public void CallingFinalize_WithParemeters_ShouldReturn_FinalRoll_WithCorrectProperties()
    {
        // Arrange
        var rolls = new List<List<int>>() 
        {
            new () {6},
            new () {6},  
            new () {10, 5}, 
            new () {7},  
            new () {9}
        };
        var preliminaryRoll = new PreliminaryRoll(rolls, true);
        preliminaryRoll.Dice.Add(new () {6});

        // Act
        var result = preliminaryRoll.FinalizePrelinaryRoll();

        // Assert
        result.Should().BeOfType<Roll>();
        result.Dice.Count.Should().Be(5);
        result.BaseDiceAreEqual.Should().BeTrue();
    }
}
