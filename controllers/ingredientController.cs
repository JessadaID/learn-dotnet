using Microsoft.AspNetCore.Mvc;
using cookapi.Data;
using cookapi.Models;

namespace cookapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController : ControllerBase
{
    private readonly ApiDbContext _context;

    public IngredientController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            count = _context.Ingredients.Count(),
            data = _context.Ingredients.ToList()
        });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var ingredient = _context.Ingredients.Find(id);
        if (ingredient == null)
        {
            return NotFound(
                new
                {
                    message = "Ingredient not found"
                }
            );
        }
        return Ok(ingredient);
    }

    [HttpPost]
    public IActionResult Post(Ingredient ingredient)
    {   
        _context.Ingredients.Add(ingredient);
        _context.SaveChanges();
        return Ok(ingredient);
    }

    [HttpPut]
    public IActionResult Put(Ingredient ingredient)
    {
        var ingredientUpdate = _context.Ingredients.Find(ingredient.Id);
        if (ingredientUpdate == null)
        {
            return NotFound(
                new
                {
                    message = "Ingredient not found"
                }
            );
        }

        ingredientUpdate.Name = ingredient.Name;
        _context.Ingredients.Update(ingredientUpdate);
        _context.SaveChanges();
        return Ok(ingredient);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        Console.WriteLine("Delete ingredient with id: " + id);
        var ingredient = _context.Ingredients.Find(id);
        if (ingredient == null)
        {
            return NotFound(
                new
                {
                    message = "Ingredient not found"
                }
            );
        }
        _context.Ingredients.Remove(ingredient);
        _context.SaveChanges();
        return Ok("Delete ingredient with id: " + id);
    }
}