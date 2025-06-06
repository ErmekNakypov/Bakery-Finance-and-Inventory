using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public interface IMeasurementUnitService
{
    Task<List<MeasurementUnit>> GetAll();
    Task<MeasurementUnit> GetMeasurementUnitById(int id);
    Task<ServiceResponse> UpdateMeasurementUnit(MeasurementUnit measurementUnit);

    Task<ServiceResponse> CreateMeasurementUnit(MeasurementUnit measurementUnit);
    Task DeleteMeasurementUnit(int id);
}