using System.Collections.Generic;
using System.Threading.Tasks;
using CodespringProject.Models;

namespace CodespringProject.Repositories.Interfaces
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient?> GetByIdAsync(int id);
        Task<IEnumerable<Ingredient>> GetByNameAsync(string name);
        Task AddAsync(Ingredient ingredient);
        Task UpdateAsync(Ingredient ingredient);
        Task DeleteAsync(int id);
    }
}