using CodespringProject.Models;
using CodespringProject.Repositories;
using CodespringProject.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CodespringProject.Services.Implementations;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IRecipeRepository _recipeRepository;

    public IngredientService(IIngredientRepository ingredientRepository, IRecipeRepository recipeRepository)
    {
        _ingredientRepository = ingredientRepository;
        _recipeRepository = recipeRepository;
    }

    public async Task<IEnumerable<Ingredient>> GetAllAsync() => await _ingredientRepository.GetAllAsync();

    public async Task<Ingredient?> GetByIdAsync(int id) => await _ingredientRepository.GetByIdAsync(id);
    public async Task AddAsync(Ingredient ingredient) => await _ingredientRepository.AddAsync(ingredient);
    public async Task<IEnumerable<IngredientDto>> GetByNameAsync(string name)
    {
        var ingredients = await _ingredientRepository.GetByNameAsync(name);

        if (ingredients.IsNullOrEmpty())
        {
            return Enumerable.Empty<IngredientDto>();
        }

        return ingredients.Select(i => new IngredientDto
        {
            Name = i.Name,
            Recipes = i.Recipes.Select(r => new RecipeShortDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList()
        }).ToList();
    }

    public async Task<RecipeDto> AddIngredientToRecipe(string recipeName, IngredientInDto ingredientDto)
    {
        // Find the existing recipe by name
        var recipe = await _recipeRepository.GetByNameAsync(recipeName);
        if (recipe == null)
        {
            throw new KeyNotFoundException("Recipe not found.");
        }

        // Check if ingredient already exists (ensure single selection)
        var ingredients = await _ingredientRepository.GetByNameAsync(ingredientDto.Name);
        var ingredient = ingredients.FirstOrDefault(); // Select the first match

        if (ingredient == null)
        {
            // Create a new ingredient if it doesn't exist
            ingredient = new Ingredient { Name = ingredientDto.Name };
            await _ingredientRepository.AddAsync(ingredient);
        }

        // Convert to List if necessary
        recipe.Ingredients = recipe.Ingredients?.ToList() ?? new List<Ingredient>();

        // Link the ingredient to the recipe if not already linked
        if (!recipe.Ingredients.Any(i => i.Id == ingredient.Id))
        {
            recipe.Ingredients.Add(ingredient);
            await _recipeRepository.UpdateAsync(recipe);
        }

        // Return updated Recipe as a DTO
        return new RecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            Image = recipe.Image,
            Ingredients = recipe.Ingredients.Select(i => new IngredientDto
            {
                Name = i.Name
            }).ToList()
        };
    }



    public async Task UpdateAsync(int id, Ingredient ingredient)
    {
        // Retrieve the existing ingredient by id.
        var existingIngredient = await _ingredientRepository.GetByIdAsync(id);
        if (existingIngredient == null)
        {
            // You can either throw an exception or return a NotFound result
            throw new Exception("Ingredient not found");
        }

        // Update the fields.
        existingIngredient.Name = ingredient.Name;

        // Call the repository to update the ingredient.
        await _ingredientRepository.UpdateAsync(existingIngredient);
    }

    public async Task DeleteAsync(int id)
    {
        await _ingredientRepository.DeleteAsync(id);
    }
}