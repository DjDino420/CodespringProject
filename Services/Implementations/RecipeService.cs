using CodespringProject.Models;
using CodespringProject.Repositories;
using CodespringProject.Repositories.Interfaces;

namespace CodespringProject.Services.Implementations;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IIngredientRepository _ingredientRepository;

    public RecipeService(IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository)
    {
        _recipeRepository = recipeRepository;
        _ingredientRepository = ingredientRepository;
    }

    public async Task<IEnumerable<RecipeListDto>> GetAllRecipesAsync()
    {
        var recipes = await _recipeRepository.GetAllAsync();

        return recipes.Select(r => new RecipeListDto
        {
            Id = r.Id,
            Name = r.Name,
            Image = r.Image
        }).ToList();
    }


    public async Task<Recipe?> GetByIdAsync(int id) => await _recipeRepository.GetByIdAsync(id);
    public async Task<IEnumerable<Recipe>> GetFilteredRecipesAsync(string? name, string? ingredientName)
    {
        return await _recipeRepository.GetFilteredRecipesAsync(name, ingredientName);
    }

    public async Task AddAsync(RecipeInDto recipeDto)
    {
        var recipe = new Recipe
        {
            Name = recipeDto.Name,
            Description = recipeDto.Description,
            Image = recipeDto.Image,
            Ingredients = new List<Ingredient>()
        };

        foreach (var ingredientDto in recipeDto.Ingredients)
        {
            // Check if ingredient already exists
            var existingIngredient = (await _ingredientRepository.GetByNameAsync(ingredientDto.Name)).FirstOrDefault();

            if (existingIngredient == null)
            {
                existingIngredient = new Ingredient { Name = ingredientDto.Name };
                await _ingredientRepository.AddAsync(existingIngredient);
            }

            // Always use the existing ingredient to prevent duplicate IDs
            recipe.Ingredients.Add(existingIngredient);
        }

        await _recipeRepository.AddAsync(recipe);
    }

    public async Task UpdateAsync(int id, Recipe recipe)
    {
        // Retrieve the existing recipe by id.
        var existingRecipe = await _recipeRepository.GetByIdAsync(id);
        if (existingRecipe == null)
        {
            // You can either throw an exception or return a NotFound result
            throw new Exception("Recipe not found");
        }

        // Update the fields.
        existingRecipe.Name = recipe.Name;
        existingRecipe.Description = recipe.Description;
        existingRecipe.Image = recipe.Image;

        // Call the repository to update the recipe.
        await _recipeRepository.UpdateAsync(existingRecipe);
    }

    public async Task DeleteAsync(int id) => await _recipeRepository.DeleteAsync(id);
}