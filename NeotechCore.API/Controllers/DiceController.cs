using Microsoft.AspNetCore.Mvc;
using NeotechCore.API.Models;

namespace NeotechCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiceController : ControllerBase
{

    private readonly ILogger<DiceController> _logger;

    public DiceController(ILogger<DiceController> logger)
    {
        _logger = logger;
    }

    [HttpGet("roll/{x}d{diceType}")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public ActionResult<DiceResponse> Post(DiceRequest request)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
