using Microsoft.AspNetCore.Mvc;
using learn_dotnet.models;
using learn_dotnet.data;

using Microsoft.AspNetCore.Authorization;

namespace learn_dotnet.controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok(new {
            count = _context.Todos.Count(),
            data = _context.Todos.ToList()
        });
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult GetById(int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo == null)
        {
            return NotFound("Todo not found");
        }
        return Ok(todo);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Todo todo)
    {
        _context.Todos.Add(todo);
        _context.SaveChanges();
        return Ok(todo);
    }
}