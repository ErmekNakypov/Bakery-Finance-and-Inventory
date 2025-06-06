using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;
public class PositionService : IPositionService
{
    private readonly IPositionRepository _repository;

    public PositionService(IPositionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Position>> GetAllPositions()
    {
        return await _repository.GetAllPositions().ToListAsync();
    }

    public async Task<Position> GetPositionById(int id)
    {
        return await _repository.GetPositionById(id);
    }

    public async Task<ServiceResponse> UpdatePosition(Position position)
    {
        var isExist = await _repository.IsExist(position);
        var response = new ServiceResponse();

        if (isExist)
        {
            response.ErrorMessage = "This position has been already added!";
            response.IsSuccess = false;
            return response;
        }

        response.IsSuccess = true;
        await _repository.UpdatePosition(position);
        return response;
    }

    public async Task<ServiceResponse> CreatePosition(Position position)
    {
        var isExist = await _repository.IsExist(position);
        var response = new ServiceResponse();
        if (isExist)
        {
            response.ErrorMessage = "This position has been already added!";
            response.IsSuccess = false;
            return response;
        }
        response.IsSuccess = true;
        await _repository.CreatePosition(position);
        return response;
    }

    public async Task DeletePosition(int id)
    {
        await _repository.DeletePosition(id);
    }
}