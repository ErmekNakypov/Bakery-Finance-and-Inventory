using Production.Models;

namespace Production.DAL.Repositories;

public interface IMeasurementUnitRepository
{
    IQueryable<MeasurementUnit> GetAll();
    Task<MeasurementUnit> GetMeasurementUnitById(int id);
    Task UpdateMeasurementUnit(MeasurementUnit measurementUnit);
    Task<bool> IsExist(MeasurementUnit measurementUnit);
    Task CreateMeasurementUnit(MeasurementUnit measurementUnit);
    Task DeleteMeasurementUnit(int id);
}