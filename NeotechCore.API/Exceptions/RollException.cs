using NeotechCore.API.Models;

namespace NeotechCore.API.Exceptions;

public class RollException : Exception
{
    public static RollException ExtraDiceRequired(StandardRollType rollType) => new($"No extra dice are allowed for '{rollType}'.");
    public static RollException NoExtraDiceAllowed(StandardRollType rollType) => new($"At least one extra die is required for '{rollType}'.");

    public RollException(string message) : base(message) { }
}
