using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class ProductManufacturingRepository : IProductManufacturingRepository
{
    
    private readonly ApplicationDbContext _context;

    public ProductManufacturingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<ProductManufacturing> GetAll()
    {
        return _context.ProductManufacturings
            .Include(x => x.Product)
            .Include(x => x.Employee);
    }

    public async Task<ProductManufacturing> GetProductManufacturingById(int id)
    {
        return await _context.ProductManufacturings.FirstOrDefaultAsync(x => x.Id == id);

    }

    public async Task UpdateProductManufacturing(ProductManufacturing productManufacturing)
    {
        _context.ProductManufacturings.Update(productManufacturing);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductManufacturing(int id)
    {
        await _context.ProductManufacturings.Where(x => x.Id == id).ExecuteDeleteAsync();

    }

    public async Task CreateProductManufacturing(ProductManufacturing product)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC ProduceProduct @ProductID, @Quantity, @EmployeeID, @ManufacturingDate",
            new SqlParameter("@ProductID", product.ProductId),
            new SqlParameter("@Quantity", product.Quantity),
            new SqlParameter("@EmployeeID", product.EmployeeId),
            new SqlParameter("@ManufacturingDate", product.ManufacturingDate)
        );
    }

    public async Task<bool> CheckRawMaterialsAmount(int productId, decimal quantity)
    {
        var isEnoughParam = new SqlParameter
        {
            ParameterName = "@IsEnough",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC CheckRawMaterialsAmount @ProductID, @Quantity, @IsEnough OUTPUT",
            new SqlParameter("@ProductID", productId),
            new SqlParameter("@Quantity", quantity),
            isEnoughParam
        );

        return (bool)isEnoughParam.Value;
    }
    
    
}