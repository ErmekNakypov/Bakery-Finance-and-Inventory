using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class ProductRepository : IProductRepository
{
    protected readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public IQueryable<FinishedProduct> GetAllProducts()
    {
        return _context.FinishedProducts
            .Include(x => x.MeasurementUnit);
    }

    public async Task<FinishedProduct> GetProductById(int id)
    {
        return await _context.FinishedProducts.Include(x => x.MeasurementUnit)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateProduct(FinishedProduct product)
    {
        _context.FinishedProducts.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task CreateProduct(FinishedProduct product)
    {
        await _context.FinishedProducts.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public int GetAmount()
    {
        return _context.FinishedProducts.Count();
    }

    public async Task DeleteProduct(int id)
    {
        await _context.FinishedProducts.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}