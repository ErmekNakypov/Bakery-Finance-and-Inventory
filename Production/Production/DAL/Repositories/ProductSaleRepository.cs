using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class ProductSaleRepository : IProductSalesRepository
{
    private readonly ApplicationDbContext _context;

    public ProductSaleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<ProductSale> GetAll()
    {
        return _context.ProductSales
            .Include(x => x.Product)
            .Include(x => x.Employee);
    }

    public async Task<ProductSale> GetProductSaleById(int id)
    {
        return await _context.ProductSales.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateProductSale(ProductSale productSale)
    {
        _context.ProductSales.Update(productSale);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductSale(int id)
    { 
        await _context.ProductSales.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task CreateProductSale(ProductSale productSale)
    {
        await _context.ProductSales.AddAsync(productSale);
        await _context.SaveChangesAsync();
    }
}