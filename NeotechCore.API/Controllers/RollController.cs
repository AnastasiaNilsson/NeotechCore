using Microsoft.AspNetCore.Mvc;
using NeotechCore.API.Actions;
using NeotechCore.API.Models;

namespace NeotechCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RollController : ControllerBase
{
    private readonly ILogger<RollController> _logger;
    public RollController(ILogger<RollController> logger) => _logger = logger;

    private bool IsBadRequest(RollOptions options)
    {
        return options switch
        {
            _ when !Enum.IsDefined(typeof(RollType), options.RollType) => true,
            _ when options.RollType == RollType.Basic && options.ExtraDice > 0 => true,
            _ when options.RollType is RollType.Auto or RollType.Flow && options.ExtraDice <= 0 => true,
            _ => false
        };
    }

    private bool IsBadRequest(int numberOfDice, int diceType)
    {
        if (numberOfDice < 1 || !Enum.IsDefined(typeof(DiceType), diceType)) return true;
        return false;
    }

    [HttpGet("{numberOfDice}d{diceType}")]
    public ActionResult<IEnumerable<int>> Get(int numberOfDice, int diceType)
    {
        if (IsBadRequest(numberOfDice, diceType)) return BadRequest();

        var diceResults = Roll.MultipleSingleDice(numberOfDice, (DiceType)diceType).Select(die => die.Result);
        return Ok(diceResults);
    }

    [HttpPost]
    public ActionResult<RollResponse> Post(RollRequest request)
    {
        if (IsBadRequest(request.Options)) return BadRequest();

        var standardRollResult = Roll.StandardRoll(request.Options);
        var rollResponse = new RollResponse()
        {
            StandardRollResults = standardRollResult,
            RequestedOptions = request.Options,
            RequestedAt = request.RequestedAt
        };

        return Ok(rollResponse);
    }
}
