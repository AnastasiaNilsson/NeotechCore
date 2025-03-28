
namespace NeotechCore.Tests.UnitTests;

public class RolledDiceTests
{

    //
    // CONSTRUCTORS & OPERATORS
    //

    [Fact]
    public void RolledDice_ShouldBe_CorrectlyInitialized()
    {
        // Arrange
        var diceList1 = Roll.MultipleSingleDice(2, DiceType.d100);
        var diceList2 = Roll.MultipleSingleDice(5, DiceType.d10);
        var modifiers1 = new RollOptions() { EdgeBonus = 10, Difficulty = 15 };
        var modifiers2 = new RollOptions() { EdgeBonus = 5, Difficulty = 25 };

        // Act
        var rolledDice1 = new StandardRoll(diceList1);
        var rolledDice2 = new StandardRoll(diceList2, modifiers1);
        var rolledDice3 = new StandardRoll(diceList1, modifiers2);

        // Assert
        rolledDice1.DicePool.Count.Should().Be(2);
        rolledDice2.DicePool.Count.Should().Be(5);
        rolledDice3.DicePool.Count.Should().Be(2);

        rolledDice1.DiceType.Should().Be(DiceType.d100);
        rolledDice2.DiceType.Should().Be(DiceType.d10);
        rolledDice3.DiceType.Should().Be(DiceType.d100);

        rolledDice1.Options.EdgeBonus.Should().Be(0);
        rolledDice2.Options.EdgeBonus.Should().Be(10);
        rolledDice3.Options.EdgeBonus.Should().Be(5);

        rolledDice1.Options.Difficulty.Should().Be(20);
        rolledDice2.Options.Difficulty.Should().Be(15);
        rolledDice3.Options.Difficulty.Should().Be(25);
    }

    [Fact]
    public void RolledDice_ShouldThrow_IfDiceListIsEmpty()
    {
        // Arrange
        var nonExistingDice = new List<SingleRolledDie>();

        // Act
        Action initialization = () => new StandardRoll(nonExistingDice);

        // Assert
        initialization.Should().Throw<RolledDiceException>().WithMessage("RolledDice cannot be initiated with an empty DiceList.");
    }

    [Fact]
    public void RolledDiceInitialization_ShouldThrow_IfDiceListContainsMultipleDiceTypes()
    {
        // Arrange
        var existingDice = new List<SingleRolledDie>()
        {
            new SingleRolledDie(DiceType.d10, 5),
            new SingleRolledDie(DiceType.d100, 55)
        };

        // Act
        Action initialization = () => new StandardRoll(existingDice);

        // Assert
        initialization.Should().Throw<RolledDiceException>().WithMessage("All dice in the DiceList must have the same DiceType.");
    }

    [Fact]
    public void AddingRolledDice_ShouldThrow_IfDiceTypesAreDifferent()
    {
        // Arrange
        var list1 = new List<SingleRolledDie>() { new SingleRolledDie(DiceType.d10, 10) };
        var list2 = new List<SingleRolledDie>() { new SingleRolledDie(DiceType.d100, 100) };
        var rolledDice1 = new StandardRoll(list1);
        var rolledDice2 = new StandardRoll(list2);

        // Act
        Action addition = () => { var newSet = rolledDice1 + rolledDice2; };

        // Assert
        addition.Should().Throw<RolledDiceException>().WithMessage("All dice in the DiceList must have the same DiceType.");
    }

    [Fact]
    public void AddingRolledDice_ShouldCorrectlyHandle_RollModifiers()
    {
        // Arrange
        var rolledDice1 = new StandardRoll(Roll.MultipleSingleDice(2), new RollOptions() { EdgeBonus = 0, Difficulty = 25 });
        var rolledDice2 = new StandardRoll(Roll.MultipleSingleDice(2), new RollOptions() { EdgeBonus = 5, Difficulty = 15 });

        // Act
        var newDice1 = rolledDice1 + rolledDice2;
        var newDice2 = rolledDice2 + rolledDice1;

        // Assert
        newDice1.Options.EdgeBonus.Should().Be(0);
        newDice1.Options.Difficulty.Should().Be(25);
        newDice2.Options.EdgeBonus.Should().Be(5);
        newDice2.Options.Difficulty.Should().Be(15);
    }


    //
    // EXTENSION METHODS
    //

    [Fact]
    public void HighestTwo_ShouldThrow_ForLessThanTwoDice()
    {
        // Arrange
        var rolledDice = new StandardRoll(Roll.MultipleSingleDice(1));

        // Act
        Action highestTwo = () => rolledDice.HighestTwo();

        // Assert
        highestTwo.Should().Throw<RolledDiceException>().WithMessage("The HighestTwo() method requires a DiceList with at least 2 dice.");
    }

    [Theory]
    [MemberData(nameof(HighestTwoTheory))]
    public void HighestTwo_ShouldReturn_HighestTwoDice(PreliminaryDiceRoll rolledDice, int[] expectedResult)
    {
        // Arrange & Act
        var highestTwo = rolledDice.HighestTwo();

        // Assert
        highestTwo.DicePool[0].Result.Should().Be(expectedResult[0]);
        highestTwo.DicePool[1].Result.Should().Be(expectedResult[1]);
    }
    public static List<object[]> HighestTwoTheory()
    {
        var rolledDice1 = new StandardRoll(TestHelper.FakeDice([1, 2, 3, 4])); int[] result1 = [4, 3];
        var rolledDice2 = new StandardRoll(TestHelper.FakeDice([4, 3, 2, 1])); int[] result2 = [4, 3];
        var rolledDice3 = new StandardRoll(TestHelper.FakeDice([1, 10, 6])); int[] result3 = [10, 6];
        var rolledDice4 = new StandardRoll(TestHelper.FakeDice([1, 2, 3, 9, 1, 8])); int[] result4 = [9, 8];

        return new List<object[]>()
        {
            {[rolledDice1, result1]},
            {[rolledDice2, result2]},
            {[rolledDice3, result3]},
            {[rolledDice4, result4]}
        };
    }

    [Fact]
    public void HighestPairOrDefault_ShouldReturnNull_IfNoPairsExist()
    {
        // Arrange
        var rolledDice = new StandardRoll(TestHelper.FakeDice([1, 2, 3, 4, 6, 7, 8, 9, 10]));

        // Act
        var result = rolledDice.HighestPairOrDefault();

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(HighestPairTheory))]
    public void HighestPairOrDefault_ShouldReturn_HighestExistingPair(PreliminaryDiceRoll rolledDice, int expectedResult)
    {
        // Arrange & Act
        var highestTwo = rolledDice.HighestPairOrDefault();

        // Assert
        highestTwo.Should().NotBeNull();
        highestTwo?.DicePool.Count.Should().Be(2);
        highestTwo?.DicePool[0].Result.Should().Be(expectedResult);
        highestTwo?.DicePool[1].Result.Should().Be(expectedResult);
    }
    public static List<object[]> HighestPairTheory()
    {
        var rolledDice1 = new StandardRoll(TestHelper.FakeDice([1, 1, 3, 8])); int result1 = 1;
        var rolledDice2 = new StandardRoll(TestHelper.FakeDice([4, 4, 7, 6, 7])); int result2 = 7;
        var rolledDice3 = new StandardRoll(TestHelper.FakeDice([1, 10, 6, 9, 4, 10, 1])); int result3 = 10;
        var rolledDice4 = new StandardRoll(TestHelper.FakeDice([1, 2, 1, 1, 1, 8, 2, 7])); int result4 = 2;

        return new List<object[]>()
        {
            {[rolledDice1, result1]},
            {[rolledDice2, result2]},
            {[rolledDice3, result3]},
            {[rolledDice4, result4]}
        };
    }
}
