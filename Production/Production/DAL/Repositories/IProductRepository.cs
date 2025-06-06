using Production.Models;

namespace Production.DAL.Repositories;

public interface IProductRepository
{
    IQueryable<FinishedProduct> GetAllProducts();

    Task<FinishedProduct> GetProductById(int id);

    Task UpdateProduct(FinishedProduct product);

    Task CreateProduct(FinishedProduct product);
    
    int GetAmount();

    Task DeleteProduct(int id);
}