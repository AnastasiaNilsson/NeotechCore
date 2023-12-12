using FluentAssertions;

namespace NeotechAPI.Tests;

public class DiceRollerTests
{
    public Dice _dice = new Dice();

    [Fact]
    public void RollOneDie_ShouldReturn_IntBetween1and10()
    {
        // Arrange
        var resultList = new List<int>();

        // Act
        foreach (var _ in Enumerable.Range(1,100))
        {
            resultList.Add(_dice.RollOne(Die.d10));
        }

        // Assert
        resultList.Should().OnlyContain(x => (1 <= x) && (x <= 10));
    }
}
