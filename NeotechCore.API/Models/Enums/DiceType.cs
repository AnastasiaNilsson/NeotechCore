using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DiceType
{
    d10 = 10,
    d100 = 100
}
