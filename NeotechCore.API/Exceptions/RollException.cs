using NeotechCore.API.Models;

namespace NeotechCore.API.Exceptions;

public class RollException : Exception
{
    public static RollException NoExtraDiceAllowed(RollType rollType) => new($"No extra dice are allowed for RollType {rollType}.");
    public static RollException ExtraDiceRequired(RollType rollType) => new($"At least one extra die is required for RollType {rollType}.");

    public RollException(string message) : base(message) { }
}
