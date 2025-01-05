using System.Text.Json.Serialization;

namespace NeotechCore.API.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RollType
{
    Basic,
    Auto,
    Flow
}
