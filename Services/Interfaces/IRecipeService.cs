using CodespringProject.Models;

namespace CodespringProject.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeListDto>> GetAllRecipesAsync();
        Task<Recipe?> GetByIdAsync(int id);
        Task<IEnumerable<Recipe>> GetFilteredRecipesAsync(string? name, string? ingredientName);
        Task AddAsync(RecipeInDto recipe);
        Task UpdateAsync(int id, Recipe recipe);
        Task DeleteAsync(int id);
    }
}