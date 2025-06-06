using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class RawMaterialRepository : IRawMaterialRepository
{
    protected readonly ApplicationDbContext _context;

    public RawMaterialRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<RawMaterial> GetAll()
    {
        return _context.RawMaterials
            .Include(x => x.MeasurementUnit);
    }

    public async Task<RawMaterial> GetRawMaterialById(int id)
    {
        return await _context.RawMaterials
            .Include(x => x.MeasurementUnit)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateRawMaterial(RawMaterial rawMaterial)
    {
         _context.RawMaterials.Update(rawMaterial);
         await _context.SaveChangesAsync();
    }

    public async Task CreateRawMaterial(RawMaterial rawMaterial)
    {
        await _context.RawMaterials.AddAsync(rawMaterial);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteRawMaterial(int id)
    {
        await _context.RawMaterials.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}