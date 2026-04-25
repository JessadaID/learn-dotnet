using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookapi.Data;
using cookapi.Models;

namespace cookapi.Controllers;

[ApiController] 
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly ApiDbContext _context;

    public RecipeController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var recipes = _context.Recipes
            .Include(r => r.RecipeIngredients)       
                .ThenInclude(ri => ri.Ingredient)    
            .ToList(); 

        return Ok(new
        {
            count = recipes.Count,
            data = recipes,
        });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var recipe = _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefault(r => r.Id == id);

        if (recipe == null)
        {
            return NotFound(new { message = "Recipe not found" });
        }

        return Ok(recipe);
    }

    [HttpPost]
    public IActionResult Post(CreateRecipeDto dto)
    {
        var recipeIngredients = new List<RecipeIngredient>();

        foreach (var ingredientDto in dto.Ingredients)
        {
            var ingredient = ResolveIngredient(ingredientDto);
            
            if (ingredient == null)
            {
                string display = !string.IsNullOrEmpty(ingredientDto.Name) ? ingredientDto.Name : $"ID {ingredientDto.Id}";
                return BadRequest(new { message = $"ไม่พบวัตถุดิบ '{display}' ในระบบ กรุณาไปเพิ่มวัตถุดิบก่อน" });
            }

            recipeIngredients.Add(new RecipeIngredient
            {
                Ingredient = ingredient,
                Quantity = ingredientDto.Quantity ?? "",
            });
        }

        var recipe = new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            Steps = dto.Steps,
            RecipeIngredients = recipeIngredients
        };

        _context.Recipes.Add(recipe);
        _context.SaveChanges(); 

        var resultRecipe = _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefault(r => r.Id == recipe.Id);

        return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, resultRecipe);
    }

    [HttpPut]
    public IActionResult Put(CreateRecipeDto dto, int id)
    {
        var recipeToUpdate = _context.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefault(r => r.Id == id);

        if (recipeToUpdate == null)
        {
            return NotFound(new { message = "Recipe not found" });
        }

        var newIngredientsLinks = new List<RecipeIngredient>();

        foreach (var ingredientDto in dto.Ingredients)
        {
            var ingredient = ResolveIngredient(ingredientDto);
            
            if (ingredient == null)
            {
                string display = !string.IsNullOrEmpty(ingredientDto.Name) ? ingredientDto.Name : $"ID {ingredientDto.Id}";
                return BadRequest(new { message = $"ไม่พบวัตถุดิบ '{display}' ในระบบ กรุณาไปเพิ่มวัตถุดิบก่อน" });
            }

            newIngredientsLinks.Add(new RecipeIngredient
            {
                RecipeId = recipeToUpdate.Id,
                IngredientId = ingredient.Id,
                Quantity = ingredientDto.Quantity ?? "",
            });
        }

        recipeToUpdate.Name = dto.Name;
        recipeToUpdate.Description = dto.Description;
        recipeToUpdate.Steps = dto.Steps;

        _context.RecipeIngredients.RemoveRange(recipeToUpdate.RecipeIngredients);
        _context.RecipeIngredients.AddRange(newIngredientsLinks);

        _context.SaveChanges();

        var resultRecipe = _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefault(r => r.Id == id);

        return Ok(resultRecipe);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var recipe = _context.Recipes.Find(id);
        
        if (recipe == null)
        {
            return NotFound(new { message = "Recipe not found" });
        }

        _context.Recipes.Remove(recipe);
        _context.SaveChanges();
        
        return Ok(new { message = "Recipe deleted successfully" });
    }

    private Ingredient? ResolveIngredient(IngredientDto dto)
    {
        Ingredient? existingIngredient = null;

        if (dto.Id > 0)
        {
            existingIngredient = _context.Ingredients.Find(dto.Id);
        }

        if (existingIngredient == null && !string.IsNullOrWhiteSpace(dto.Name))
        {
            existingIngredient = _context.Ingredients
                .FirstOrDefault(i => i.Name.ToLower() == dto.Name.ToLower());
        }

        return existingIngredient;
    }
}