using NeotechCore.API.Exceptions;

namespace NeotechCore.Tests.UnitTests;

public class DiceSetTests
{

    //
    // CONSTRUCTORS & OPERATORS
    //

    [Fact]
    public void DiceSet_ShouldBe_CorrectlyInitialized()
    {
        // Arrange
        var rolledDice1 = TestHelper.ManyDice(2, DiceType.d100);
        var rolledDice2 = TestHelper.ManyDice(5, DiceType.d10);
        var modifiers1 = new RollModifiers(rollBonus: 10, difficulty: 15);
        var modifiers2 = new RollModifiers(rollBonus: 5, difficulty: 25);

        // Act
        var diceSet1 = new DiceSet(rolledDice1);
        var diceSet2 = new DiceSet(rolledDice2, modifiers1);
        var diceSet3 = new DiceSet(diceSet1, modifiers2);

        // Assert
        diceSet1.Dice.Count.Should().Be(2);
        diceSet2.Dice.Count.Should().Be(5);
        diceSet3.Dice.Count.Should().Be(2);

        diceSet1.DiceType.Should().Be(DiceType.d100);
        diceSet2.DiceType.Should().Be(DiceType.d10);
        diceSet3.DiceType.Should().Be(DiceType.d100);

        diceSet1.Modifiers.RollBonus.Should().Be(0);
        diceSet2.Modifiers.RollBonus.Should().Be(10);
        diceSet3.Modifiers.RollBonus.Should().Be(5);

        diceSet1.Modifiers.Difficulty.Should().Be(20);
        diceSet2.Modifiers.Difficulty.Should().Be(15);
        diceSet3.Modifiers.Difficulty.Should().Be(25);
    }

    [Fact]
    public void DiceSet_ShouldThrow_IfListIsEmpty()
    {
        // Arrange
        var nonExistingDice = new List<RolledDie>();

        // Act
        Action initialization = () => new DiceSet(nonExistingDice);

        // Assert
        initialization.Should().Throw<RollException>().WithMessage("A DiceSet cannot be created with an empty list.");
    }

    [Fact]
    public void DiceSet_ShouldThrow_IfListContainsMultipleDiceTypes()
    {
        // Arrange
        var existingDice = new List<RolledDie>()
        {
            new RolledDie(DiceType.d10, 5),
            new RolledDie(DiceType.d100, 55)
        };

        // Act
        Action initialization = () => new DiceSet(existingDice);

        // Assert
        initialization.Should().Throw<RollException>().WithMessage("All dice in a DiceSet must have the same DiceType.");
    }

    [Fact]
    public void AddingDiceSets_ShouldThrow_IfDiceTypesAreDifferent()
    {
        // Arrange
        var list1 = new List<RolledDie>() { new RolledDie(DiceType.d10, 10) };
        var list2 = new List<RolledDie>() { new RolledDie(DiceType.d100, 100) };
        var diceSet1 = new DiceSet(list1);
        var diceSet2 = new DiceSet(list2);

        // Act
        Action addition = () => { var newSet = diceSet1 + diceSet2; };

        // Assert
        addition.Should().Throw<RollException>().WithMessage("All dice in a DiceSet must have the same DiceType.");
    }

    [Fact]
    public void AddingDiceSets_ShouldCorrectlyHandle_RollModifiers()
    {
        // Arrange
        var diceSet1 = new DiceSet(TestHelper.ManyDice(2), new RollModifiers(0, 25));
        var diceSet2 = new DiceSet(TestHelper.ManyDice(2), new RollModifiers(5, 20));
        var diceSet3 = new DiceSet(TestHelper.ManyDice(2), new RollModifiers(6, 20));
        var diceSet4 = new DiceSet(TestHelper.ManyDice(2), new RollModifiers(7, 15));

        // Act
        var newSet1 = diceSet1 + diceSet2;
        var newSet2 = diceSet3 + diceSet4;

        // Assert
        newSet1.Modifiers.RollBonus.Should().Be(5);
        newSet1.Modifiers.Difficulty.Should().Be(25);
        newSet2.Modifiers.RollBonus.Should().Be(6);
        newSet2.Modifiers.Difficulty.Should().Be(15);
    }


    //
    // EXTENSION METHODS
    //

    [Fact]
    public void HighestTwo_ShouldThrow_ForLessThanTwoDice()
    {
        // Arrange
        var diceSet = new DiceSet(TestHelper.ManyDice(1));

        // Act
        Action highestTwo = () => diceSet.HighestTwo();

        // Assert
        highestTwo.Should().Throw<RollException>().WithMessage("The HighestTwo() method requires a DiceSet with at least 2 dice.");
    }

    [Theory]
    [MemberData(nameof(HighestTwoTheory))]
    public void HighestTwo_ShouldReturn_HighestTwoDice(DiceSet diceSet, int[] expectedResult)
    {
        // Arrange & Act
        var highestTwo = diceSet.HighestTwo();

        // Assert
        highestTwo.Dice[0].Result.Should().Be(expectedResult[0]);
        highestTwo.Dice[1].Result.Should().Be(expectedResult[1]);
    }
    public static List<object[]> HighestTwoTheory()
    {
        var diceSet1 = new DiceSet(TestHelper.FakeDice([1, 2, 3, 4])); int[] result1 = [4, 3];
        var diceSet2 = new DiceSet(TestHelper.FakeDice([4, 3, 2, 1])); int[] result2 = [4, 3];
        var diceSet3 = new DiceSet(TestHelper.FakeDice([1, 10, 6])); int[] result3 = [10, 6];
        var diceSet4 = new DiceSet(TestHelper.FakeDice([1, 2, 3, 9, 1, 8])); int[] result4 = [9, 8];

        return new List<object[]>()
        {
            {[diceSet1, result1]},
            {[diceSet2, result2]},
            {[diceSet3, result3]},
            {[diceSet4, result4]}
        };
    }

    [Fact]
    public void HighestPairOrDefault_ShouldReturnNull_IfNoPairsExist()
    {
        // Arrange
        var diceSet = new DiceSet(TestHelper.FakeDice([1, 2, 3, 4, 6, 7, 8, 9, 10]));

        // Act
        var result = diceSet.HighestPairOrDefault();

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(HighestPairTheory))]
    public void HighestPairOrDefault_ShouldReturn_HighestExistingPair(DiceSet diceSet, int expectedResult)
    {
        // Arrange & Act
        var highestTwo = diceSet.HighestPairOrDefault();

        // Assert
        highestTwo.Should().NotBeNull();
        highestTwo?.Dice.Count.Should().Be(2);
        highestTwo?.Dice[0].Result.Should().Be(expectedResult);
        highestTwo?.Dice[1].Result.Should().Be(expectedResult);
    }
    public static List<object[]> HighestPairTheory()
    {
        var diceSet1 = new DiceSet(TestHelper.FakeDice([1, 1, 3, 8])); int result1 = 1;
        var diceSet2 = new DiceSet(TestHelper.FakeDice([4, 4, 7, 6, 7])); int result2 = 7;
        var diceSet3 = new DiceSet(TestHelper.FakeDice([1, 10, 6, 9, 4, 10, 1])); int result3 = 10;
        var diceSet4 = new DiceSet(TestHelper.FakeDice([1, 2, 1, 1, 1, 8, 2, 7])); int result4 = 2;

        return new List<object[]>()
        {
            {[diceSet1, result1]},
            {[diceSet2, result2]},
            {[diceSet3, result3]},
            {[diceSet4, result4]}
        };
    }
}
