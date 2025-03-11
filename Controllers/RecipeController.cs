using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodespringProject.Models;
using CodespringProject.Services;

namespace CodespringProject.Controllers
{

    [Route("api/recipes")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeListDto>>> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null) return NotFound();
            return Ok(recipe);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetRecipes([FromQuery] string? name, [FromQuery] string? ingredient)
        {
            var recipes = await _recipeService.GetFilteredRecipesAsync(name, ingredient);
            return Ok(recipes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeInDto recipe)
        {
            await _recipeService.AddAsync(recipe);
            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] Recipe recipe)
        {
            try
            {
                await _recipeService.UpdateAsync(id, recipe);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeService.DeleteAsync(id);
            return NoContent();
        }
    }
}