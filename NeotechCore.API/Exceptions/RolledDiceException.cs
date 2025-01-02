namespace NeotechCore.API.Exceptions;

public class RolledDiceException : Exception
{
    public static RolledDiceException EmptyList { get; } = new("RolledDice cannot be initiated with an empty DiceList.");
    public static RolledDiceException MultipleDiceTypes { get; } = new RolledDiceException("All dice in the DiceList must have the same DiceType.");
    public static RolledDiceException HighestTwoRequiresTwo { get; } = new RolledDiceException("The HighestTwo() method requires a DiceList with at least 2 dice.");

    public RolledDiceException(string message) : base(message) { }
}
