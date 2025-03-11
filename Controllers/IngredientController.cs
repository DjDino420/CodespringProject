using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodespringProject.Models;
using CodespringProject.Services;

namespace CodespringProject.Controllers
{

    [Route("api/ingredients")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
        {
            return Ok(await _ingredientService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(int id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null) return NotFound();
            return Ok(ingredient);
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredientsByName(string name)
        {
            var ingredients = await _ingredientService.GetByNameAsync(name);

            if (!ingredients.Any())  // Check if no ingredients were found
            {
                return NotFound(new { message = "No ingredients found with the given name" });
            }

            return Ok(ingredients);
        }

        [HttpPost("{recipeName}/add-ingredient")]
        public async Task<IActionResult> AddIngredientToRecipe(string recipeName, [FromBody] IngredientInDto ingredientDto)
        {
            try
            {
                var updatedRecipe = await _ingredientService.AddIngredientToRecipe(recipeName, ingredientDto);
                return Ok(updatedRecipe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredient([FromBody] Ingredient ingredient)
        {
            await _ingredientService.AddAsync(ingredient);
            return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.Id }, ingredient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] Ingredient ingredient)
        {
            try
            {
                await _ingredientService.UpdateAsync(id, ingredient);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _ingredientService.DeleteAsync(id);
            return NoContent();
        }
    }
}