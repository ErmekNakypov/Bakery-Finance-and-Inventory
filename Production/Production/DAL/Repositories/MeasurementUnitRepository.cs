using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class MeasurementUnitRepository : IMeasurementUnitRepository
{
    protected readonly ApplicationDbContext _context;

    public MeasurementUnitRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<MeasurementUnit> GetAll()
    {
        return _context.MeasurementUnits;
    }

    public async Task<MeasurementUnit> GetMeasurementUnitById(int id)
    {
        return await _context.MeasurementUnits.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateMeasurementUnit(MeasurementUnit measurementUnit)
    {
        _context.MeasurementUnits.Update(measurementUnit);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExist(MeasurementUnit measurementUnit)
    {
        return await _context.MeasurementUnits.AnyAsync(x => x.Name.Equals(measurementUnit.Name));
    }

    public async Task CreateMeasurementUnit(MeasurementUnit measurementUnit)
    {
        await _context.MeasurementUnits.AddAsync(measurementUnit);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMeasurementUnit(int id)
    {
        await _context.MeasurementUnits.Where(x => x.Id == id).ExecuteDeleteAsync();

    }
}