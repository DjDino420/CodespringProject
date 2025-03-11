using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodespringProject.Models;
using CodespringProject.Repositories.Interfaces;
using CodespringProject.Data;


namespace CodespringProject.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _context;

        public RecipeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _context.Recipes.AsNoTracking().ToListAsync();
        }


        public async Task<Recipe?> GetByIdAsync(int id)
        {
            return await _context.Recipes.Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<Recipe?> GetByNameAsync(string name)
        {
            return await _context.Recipes.Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Name == name);
        }
        public async Task<IEnumerable<Recipe>> GetFilteredRecipesAsync(string? name, string? ingredientName)
        {
            var query = _context.Recipes.Include(r => r.Ingredients).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(r => r.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(ingredientName))
            {
                query = query.Where(r => r.Ingredients.Any(i => i.Name.Contains(ingredientName)));
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }
    }
}