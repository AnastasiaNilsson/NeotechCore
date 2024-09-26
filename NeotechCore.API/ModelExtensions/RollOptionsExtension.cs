using NeotechCore.API.Models;
using NeotechCore.API.Exceptions;

namespace NeotechCore.API.ModelExtensions;

public static class RollOptionsExtension
{
    public static uint RollBonus(this RollOptions options) => options.AttributeScore + options.EdgeBonus;
}
