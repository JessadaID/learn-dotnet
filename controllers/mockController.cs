using Microsoft.AspNetCore.Mvc;

namespace cookapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }

    [HttpPost]
    public IActionResult Post(string name)
    {
        return Ok("Hello post " + name);
    }

    [HttpPut]
    public IActionResult Put(string name)
    {
        return Ok("Hello put " + name);
    }

    [HttpDelete]
    public IActionResult Delete(string name)
    {
        return Ok("Hello delete " + name);
    }
}