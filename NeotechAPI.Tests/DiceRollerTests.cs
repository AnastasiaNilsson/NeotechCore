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
        result.Dice.Count().Should().Be(2);
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
        result.Dice.Count().Should().Be(7);
        result.UseAutoDice.Should().BeTrue();
    }
}
