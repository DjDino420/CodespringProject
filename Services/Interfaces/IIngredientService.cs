using CodespringProject.Models;

namespace CodespringProject.Services
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient?> GetByIdAsync(int id);
        Task<IEnumerable<IngredientDto>> GetByNameAsync(string name);
        Task AddAsync(Ingredient ingredient);
        Task<RecipeDto> AddIngredientToRecipe(string recipeName, IngredientInDto ingredientDto);
        Task UpdateAsync(int id, Ingredient ingredient);
        Task DeleteAsync(int id);
    }
}