using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public class MeasurementUnitService : IMeasurementUnitService
{
    private readonly IMeasurementUnitRepository _repository;

    public MeasurementUnitService(IMeasurementUnitRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MeasurementUnit>> GetAll()
    {
        return await _repository.GetAll().ToListAsync();
    }

    public async Task<MeasurementUnit> GetMeasurementUnitById(int id)
    {
        return await _repository.GetMeasurementUnitById(id);
    }

    public async Task<ServiceResponse> UpdateMeasurementUnit(MeasurementUnit measurementUnit)
    {
        var isExist = await _repository.IsExist(measurementUnit);
        var response = new ServiceResponse();

        if (isExist)
        {
            response.ErrorMessage = "This measurement unit has been already added!";
            response.IsSuccess = false;
            return response;
        }

        response.IsSuccess = true;
        await _repository.UpdateMeasurementUnit(measurementUnit);
        return response;
    }
    
    public async Task<ServiceResponse> CreateMeasurementUnit(MeasurementUnit measurementUnit)
    {
        var isExist = await _repository.IsExist(measurementUnit);
        var response = new ServiceResponse();
        if (isExist)
        {
            response.ErrorMessage = "This  measurement unit has been already added!";
            response.IsSuccess = false;
            return response;
        }
        response.IsSuccess = true;
        await _repository.CreateMeasurementUnit(measurementUnit);
        return response;
    }

    public async Task DeleteMeasurementUnit(int id)
    {
        await _repository.DeleteMeasurementUnit(id);
    }
}