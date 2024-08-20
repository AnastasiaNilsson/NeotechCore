using Neotech.Extensions;
using Neotech.Models;

namespace NeotechCore.Tests.UnitTests;

public class DiceTests
{

    public Die[] GetDice(int numberOfDice, DiceType diceType)
    {
        var diceArray = new Die[numberOfDice];
        return diceArray.Select(die => new Die(diceType)).ToArray();
    }

    public static Die[] FakeDice(int[] results, DiceType diceType = DiceType.d10)
    {
        var diceCollection = new List<Die>();
        
        foreach (var result in results)
        {
            var fake = Mock.Of<Die>(die => die.Result == result && die.DiceType == diceType);
            diceCollection.Add(fake);
        }

        return diceCollection.ToArray();
    }


    //
    // CORE DICE FUNCTIONALITY
    //

    [Fact]
    public void DiceType_ShouldMatch_ParameterDiceType()
    {
        // Arrange & Act
        var die = new Die(DiceType.d100);

        // Assert
        die.DiceType.Should().Be(DiceType.d100);
    }

    [Fact]
    public void Result_ShouldBeWithin_DiceTypeMaxValue()
    {
        // Arrange & Act
        var diceArray = GetDice(1000, DiceType.d10);

        // Assert
        foreach (var die in diceArray)
        {
            die.Result.Should().BeInRange(1, (int)DiceType.d10);
        }
    }

    [Fact]
    public void DiceSet_ShouldBe_CorrectlyInitialized()
    {
        // Arrange & Act
        var diceSet1 = new DiceSet(5);
        var diceSet2 = new DiceSet(10, 10, 10);
        var diceSet3 = new DiceSet(numberOfDice: 1, diceType: DiceType.d100);

        // Assert
        diceSet1.Dice.Length.Should().Be(5);
        diceSet1.DiceType.Should().Be(DiceType.d10);
        diceSet1.RollBonus.Should().Be(0);
        diceSet1.Difficulty.Should().Be(20);

        diceSet2.Dice.Length.Should().Be(10);
        diceSet2.DiceType.Should().Be(DiceType.d10);
        diceSet2.RollBonus.Should().Be(10);
        diceSet2.Difficulty.Should().Be(10);

        diceSet3.Dice.Length.Should().Be(1);
        diceSet3.DiceType.Should().Be(DiceType.d100);
        diceSet3.RollBonus.Should().Be(0);
        diceSet3.Difficulty.Should().Be(20);
    }

    [Fact]
    public void DiceSet_InitializedFromExistingDice_ShouldBe_CorrectlyInitialized()
    {
        // Arrange
        var existingDice1 = GetDice(5, DiceType.d10);
        var existingDice2 = GetDice(2, DiceType.d100);

        // Act
        var diceSet1 = new DiceSet(existingDice1);
        var diceSet2 = new DiceSet(existingDice2, rollBonus: 10, difficulty: 10);

        // Assert
        diceSet1.Dice.Length.Should().Be(5);
        diceSet1.DiceType.Should().Be(DiceType.d10);
        diceSet1.RollBonus.Should().Be(0);
        diceSet1.Difficulty.Should().Be(20);

        diceSet2.Dice.Length.Should().Be(2);
        diceSet2.DiceType.Should().Be(DiceType.d100);
        diceSet2.RollBonus.Should().Be(10);
        diceSet2.Difficulty.Should().Be(10);
    }

    [Fact]
    public void DiceSet_InitializedFromExistingDice_ShouldThrow_IfArrayIsEmpty()
    {
        // Arrange
        var nonExistingDice1 = new Die[0];
        var nonExistingDice2 = new Die[5];

        // Act
        Action initialization1 = () => new DiceSet(nonExistingDice1);
        Action initialization2 = () => new DiceSet(nonExistingDice1);

        // Assert
        initialization1.Should().Throw<ArgumentException>().WithMessage("A DiceSet cannot be created with an empty array.");
        initialization2.Should().Throw<ArgumentException>().WithMessage("A DiceSet cannot be created with an empty array.");
    }

    [Fact]
    public void DiceSet_InitializedFromExistingDice_ShouldThrow_IfArrayContainsMultipleDiceTypes()
    {
        // Arrange
        var existingDice = new Die[2];
        existingDice[0] = new Die(DiceType.d10);
        existingDice[1] = new Die(DiceType.d100);

        // Act
        Action initialization = () => new DiceSet(existingDice);

        // Assert
        initialization.Should().Throw<ArgumentException>().WithMessage("All dice in a DiceSet must have the same DiceType.");
    }

    [Fact]
    public void AddingDiceSets_ShouldThrow_IfDiceTypesAreDirrent()
    {
        // Arrange
        var diceSet1 = new DiceSet(2, diceType: DiceType.d10);
        var diceSet2 = new DiceSet(1, diceType: DiceType.d100);
        
        // Act
        Action addition = () => { var newSet = diceSet1 + diceSet2; };

        // Assert
        addition.Should().Throw<ArgumentException>().WithMessage("All dice in a DiceSet must have the same DiceType.");
    }

    [Fact]
    public void AddingDiceSets_ShouldTake_TheHigherDifficulty()
    {
        // Arrange
        var diceSet1 = new DiceSet(2, difficulty: 20);
        var diceSet2 = new DiceSet(1, difficulty: 32);
        var diceSet3 = new DiceSet(1, difficulty: 24);
        var diceSet4 = new DiceSet(1, difficulty: 16);
        
        // Act
        var newSet1 = diceSet1 + diceSet2;
        var newSet2 = diceSet3 + diceSet4;

        // Assert
        newSet1.Difficulty.Should().Be(32);
        newSet2.Difficulty.Should().Be(24);
    }


    //
    // DICESET EXTENSION METHODS
    //

    [Fact]
    public void HighestTwo_ShouldThrow_ForLessThanTwoDice()
    {
        // Arrange
        var diceSet = new DiceSet(1);

        // Act
        Action highestTwo = () => diceSet.HighestTwo();

        // Assert
        highestTwo.Should().Throw<ArgumentException>().WithMessage("The HighestTwo() method requires a DiceSet with at least 2 dice.");
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
        var diceSet1 = new DiceSet(FakeDice([1, 2, 3, 4]       )); int[] result1 = [4, 3];
        var diceSet2 = new DiceSet(FakeDice([4, 3, 2, 1]       )); int[] result2 = [4, 3];
        var diceSet3 = new DiceSet(FakeDice([1, 10, 6]         )); int[] result3 = [10, 6];
        var diceSet4 = new DiceSet(FakeDice([1, 2, 3, 9, 1, 8] )); int[] result4 = [9, 8];

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
        var diceSet = new DiceSet(FakeDice([1, 2, 3, 4, 6, 7, 8, 9, 10]));
     
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
        highestTwo?.Dice.Length.Should().Be(2);
        highestTwo?.Dice[0].Result.Should().Be(expectedResult);
        highestTwo?.Dice[1].Result.Should().Be(expectedResult);
    }
    public static List<object[]> HighestPairTheory()
    {     
        var diceSet1 = new DiceSet(FakeDice([1, 1, 3, 8]             )); int result1 = 1;
        var diceSet2 = new DiceSet(FakeDice([4, 4, 7, 6, 7]          )); int result2 = 7;
        var diceSet3 = new DiceSet(FakeDice([1, 10, 6, 9, 4, 10, 1]  )); int result3 = 10;
        var diceSet4 = new DiceSet(FakeDice([1, 2, 1, 1, 1, 8, 2, 7] )); int result4 = 2;

        return new List<object[]>()
        {
            {[diceSet1, result1]},
            {[diceSet2, result2]},
            {[diceSet3, result3]},
            {[diceSet4, result4]}
        };
    }

    [Theory]
    [MemberData(nameof(ExplosionTheory))]
    public void Explosion_ShouldAdd_CorrectNumberOfDice(DiceSet diceSet, bool doubleChanceStatus)
    {
        // Arrange
        var originalDiceCount = diceSet.Dice.Length;

        // Act
        diceSet += diceSet.Explosion(doubleChanceStatus);
        
        var explosions = doubleChanceStatus ? 
                         diceSet.Dice.Where(die => die.Result >= 9).ToArray() :
                         diceSet.Dice.Where(die => die.Result == 10).ToArray();
        
        var explosionCount = explosions.Length;
        var totalDiceCount = diceSet.Dice.Length;

        // Assert
        totalDiceCount.Should().Be(originalDiceCount + explosionCount);
    }
    public static List<object[]> ExplosionTheory()
    {     
        var diceSet1 = new DiceSet(FakeDice([10]              ));
        var diceSet2 = new DiceSet(FakeDice([10, 1, 3, 8]     ));
        var diceSet3 = new DiceSet(FakeDice([1, 9, 9, 2]      ));
        var diceSet4 = new DiceSet(FakeDice([1, 10, 4, 10, 1] ));

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

}
