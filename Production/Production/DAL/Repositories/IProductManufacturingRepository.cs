using Production.Models;

namespace Production.DAL.Repositories;

public interface IProductManufacturingRepository
{
    IQueryable<ProductManufacturing> GetAll();
    Task<ProductManufacturing> GetProductManufacturingById(int id);
    Task UpdateProductManufacturing(ProductManufacturing productManufacturing);
    Task DeleteProductManufacturing(int id);
    Task CreateProductManufacturing(ProductManufacturing productManufacturing);
    Task<bool> CheckRawMaterialsAmount(int productId, decimal quantity);
}