using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class IngredientRepository : IIngredientsRepository
{
    protected readonly ApplicationDbContext _context;

    public IngredientRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<Ingredient> GetIngredientsByProductId(long id)
    {
        return _context.Ingredients
            .Include(x => x.RawMaterial)
            .Where(x => x.ProductId == id);
    }

    public Task<Ingredient> GetIngredientById(int id)
    {
        return _context.Ingredients
            .Include(x => x.RawMaterial)
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateIngredient(Ingredient ingredient)
    {
        _context.Ingredients.Update(ingredient);
        await _context.SaveChangesAsync();
    }

    public async Task CreateIngredient(Ingredient ingredient)
    {
        await _context.Ingredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteIngredient(int id)
    {
        await _context.Ingredients.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<bool> IsExist(Ingredient ingredient)
    {
        return await _context.Ingredients.AnyAsync(x =>
            x.ProductId == ingredient.ProductId && x.RawMaterialId == ingredient.RawMaterialId);
    }
}