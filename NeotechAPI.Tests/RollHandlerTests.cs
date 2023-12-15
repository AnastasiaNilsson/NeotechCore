using FluentAssertions;
using Services.Dice;

namespace NeotechAPI.Tests;

public class RollHandlerTests
{
    [Fact]
    public void FinalizePrelinaryRoll_ShouldReturn_Roll()
    {
        // Arrange
        var dice = new int[] {6, 9};
        var preliminaryRoll = new PreliminaryRoll(dice, false);

        // Act
        var roll = preliminaryRoll.FinalizePrelinaryRoll();

        // Assert
        roll.Should().BeOfType<FinalRoll>();
        roll.Dice.Count().Should().Be(2);
    }

    [Fact]
    public void FinalizePrelinaryRoll_WithNoParameters_ShouldReturn_HighestRoll()
    {
        // Arrange
        var dice = new int[] {6, 9, 1};
        var preliminaryRoll = new PreliminaryRoll(dice, false);

        // Act
        var roll = preliminaryRoll.FinalizePrelinaryRoll();

        // Assert
        roll.Dice.Count().Should().Be(2);
        roll.Dice[0].Should().Be(6);
        roll.Dice[1].Should().Be(9);
    }

    [Fact]
    public void FinalizePrelinaryRoll_WithAutoDice_ShouldReturn_HighestRoll()
    {
        // Arrange
        var dice = new int[] {9, 6, 1, 10, 9};
        var preliminaryRoll = new PreliminaryRoll(dice, true);

        // Act
        var roll = preliminaryRoll.FinalizePrelinaryRoll();

        // Assert
        roll.Dice.Count().Should().BeGreaterThanOrEqualTo(3);
        roll.Dice[0].Should().Be(9);
        roll.Dice[1].Should().Be(10);
    }

        [Fact]
    public void FinalizePrelinaryRoll_WithAutoDice_ShouldReturn_HighestRoll()
    {
        // Arrange
        var dice = new int[] {9, 6, 1, 10, 9};
        var preliminaryRoll = new PreliminaryRoll(dice, true);

        // Act
        var roll = preliminaryRoll.FinalizePrelinaryRoll();

        // Assert
        roll.Dice.Count().Should().BeGreaterThanOrEqualTo(3);
        roll.Dice[0].Should().Be(9);
        roll.Dice[1].Should().Be(10);
    }
}
