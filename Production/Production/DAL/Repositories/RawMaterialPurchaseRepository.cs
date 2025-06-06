using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class RawMaterialPurchaseRepository: IRawMaterialPurchaseRepository
{
    private readonly ApplicationDbContext _context;

    public RawMaterialPurchaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<RawMaterialPurchase> GetAll()
    {
        return _context.RawMaterialPurchases
            .Include(x => x.Employee)
            .Include(x => x.RawMaterial);
    }

    public async Task<RawMaterialPurchase> GetRawMaterialPurchaseById(int id)
    {
        return await _context.RawMaterialPurchases.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateRawMaterialPurchase(RawMaterialPurchase rawMaterialPurchase)
    {
        _context.RawMaterialPurchases.Update(rawMaterialPurchase);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRawMaterialPurchase(int id)
    {
        await _context.RawMaterialPurchases.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task CreateRawMaterialPurchase(RawMaterialPurchase purchase)
    {
        await _context.RawMaterialPurchases.AddAsync(purchase);
        await _context.SaveChangesAsync();
    }
}