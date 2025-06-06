using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public interface IIngredientService
{ 
    Task<List<Ingredient>> GetIngredientsByProductId(long id);
    Task<Ingredient> GetIngredientById(int id);
    Task UpdateIngredient(Ingredient ingredient);
    Task<ServiceResponse> CreateIngredient(Ingredient ingredient);
    Task DeleteIngredient(int id);

}