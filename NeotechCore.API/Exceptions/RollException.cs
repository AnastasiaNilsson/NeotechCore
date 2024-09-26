using NeotechCore.API.Models;

namespace NeotechCore.API.Exceptions;

public class RollException : Exception
{
    public static RollException ExtraDiceRequired(RollType rollType) => new($"No extra dice are allowed for '{rollType}'.");
    public static RollException NoExtraDiceAllowed(RollType rollType) => new($"At least one extra die is required for '{rollType}'.");

    public RollException(string message) : base(message) { }
}
