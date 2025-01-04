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


    [HttpGet("{numberOfDice}d{diceType}")]
    public ActionResult<IEnumerable<int>> Get(int numberOfDice, int diceType)
    {
        if (numberOfDice < 1 || !Enum.IsDefined(typeof(DiceType), diceType)) return BadRequest();

        try
        {
            var diceResults = Roll.MultipleDice(numberOfDice, (DiceType)diceType).Select(die => die.Result);
            return Ok(diceResults);
        }
        catch
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    // [HttpPost]
    // public ActionResult<DiceResponse> Post(RollRequest request)
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //     {
    //         Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    // }
}
