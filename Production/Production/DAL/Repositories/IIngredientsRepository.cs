using Production.Models;

namespace Production.DAL.Repositories;

public interface IIngredientsRepository
{ 
    IQueryable<Ingredient> GetIngredientsByProductId(long id);
    Task<Ingredient> GetIngredientById(int id);
    Task UpdateIngredient(Ingredient ingredient);
    Task CreateIngredient(Ingredient ingredient);
    Task DeleteIngredient(int id);
    Task<bool> IsExist(Ingredient ingredient);
}