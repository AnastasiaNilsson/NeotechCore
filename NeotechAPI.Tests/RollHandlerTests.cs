using FluentAssertions;
using Services.Dice;

namespace NeotechAPI.Tests;

public class DiceHandlerests
{
    [Fact]
    public void FinalizePrelinaryRoll_ShouldReturn_Roll()
    {
        // Arrange
        var dice = new int[][] 
        {
            new int[] {6},
            new int[] {9},
        };
        var preliminaryRoll = new PreliminaryRoll(dice, false);

        // Act
        var roll = preliminaryRoll.FinalizePrelinaryRoll();

        // Assert
        roll.Should().BeOfType<Roll>();
        roll.Dice.Count().Should().Be(2);
    }

    [Fact]
    public void FinalizePrelinaryRoll_WithNoParameters_ShouldReturn_HighestRoll()
    {
        // Arrange
        var dice = new int[][] 
        {
            new int[] {6},
            new int[] {9},
            new int[] {1},
        };
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
        var dice = new int[][] 
        {
            new int[] {9},
            new int[] {6},
            new int[] {1}, 
            new int[] {10, 1}, 
            new int[] {9}, 
        };
        var preliminaryRoll = new PreliminaryRoll(dice, true);

        // Act
        var roll = preliminaryRoll.FinalizePrelinaryRoll();

        // Assert
        roll.Dice.Count().Should().Be(3);
        roll.BaseDiceAreEqual.Should().BeFalse();
    }
}
