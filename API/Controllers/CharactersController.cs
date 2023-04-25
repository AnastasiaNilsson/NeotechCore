using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
    public CharactersController()
    {

    }

    [HttpGet]
    public IEnumerable<int> Get()
    {
        return new List<int>();
    }
}
