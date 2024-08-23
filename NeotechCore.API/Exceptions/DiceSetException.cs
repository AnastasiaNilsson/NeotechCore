namespace NeotechCore.API.Exceptions;

public class DiceSetException : Exception
{
    public static DiceSetException EmptyList { get; } = new DiceSetException("A DiceSet cannot be created with an empty list.");
    public static DiceSetException MultipleDiceTypes { get; } = new DiceSetException("All dice in a DiceSet must have the same DiceType.");
    public static DiceSetException HighestTwoRequiresTwo { get; } = new DiceSetException("The HighestTwo() method requires a DiceSet with at least 2 dice.");

    public DiceSetException(string message) : base(message) {}
}
