using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodespringProject.Models;
using CodespringProject.Repositories.Interfaces;
using CodespringProject.Data;


namespace CodespringProject.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly ApplicationDbContext _context;

        public IngredientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<Ingredient?> GetByIdAsync(int id)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Ingredient>> GetByNameAsync(string name)
        {
            return await _context.Ingredients
                .Where(i => i.Name.Contains(name))
                .Include(i => i.Recipes)  // Include related recipes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ingredient = await _context.Ingredients.Include(i => i.Recipes).FirstOrDefaultAsync(i => i.Id == id);
            if (ingredient == null) return;

            // Töröljük a hozzávalót minden receptből
            foreach (var recipe in ingredient.Recipes.ToList())
            {
                recipe.Ingredients.Remove(ingredient);
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }

    }
}