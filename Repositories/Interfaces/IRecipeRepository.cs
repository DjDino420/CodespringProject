using System.Collections.Generic;
using System.Threading.Tasks;
using CodespringProject.Models;

namespace CodespringProject.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<Recipe?> GetByIdAsync(int id);
        Task<Recipe?> GetByNameAsync(string name);
        Task<IEnumerable<Recipe>> GetFilteredRecipesAsync(string? name, string? ingredientName);
        Task AddAsync(Recipe recipe);
        Task UpdateAsync(Recipe recipe);
        Task DeleteAsync(int id);
    }
}