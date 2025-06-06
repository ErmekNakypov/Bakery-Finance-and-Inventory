using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public class IngredientService : IIngredientService
{
    private readonly IIngredientsRepository _repository;

    public IngredientService(IIngredientsRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<Ingredient>> GetIngredientsByProductId(long id)
    {
        return await _repository.GetIngredientsByProductId(id).ToListAsync();
    }

    public async Task<Ingredient> GetIngredientById(int id)
    {
        return await _repository.GetIngredientById(id);
    }

    public async Task UpdateIngredient(Ingredient ingredient)
    {
         await _repository.UpdateIngredient(ingredient);
    }

    public async Task<ServiceResponse> CreateIngredient(Ingredient ingredient)
    {
        var isExist = await _repository.IsExist(ingredient);
        var response = new ServiceResponse();
        
        if (isExist)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "This ingredient has been already added!";
            return response;
        }

        response.IsSuccess = true;
        await _repository.CreateIngredient(ingredient);
        return response;
    }

    public async Task DeleteIngredient(int id)
    {
        await _repository.DeleteIngredient(id);
    }
}