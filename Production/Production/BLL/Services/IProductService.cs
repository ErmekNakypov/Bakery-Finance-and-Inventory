using Production.Models;

namespace Production.BLL.Services;

public interface IProductService
{
    Task<List<FinishedProduct>> GetAllProducts();

    Task<FinishedProduct> GetProductById(int id);

    Task UpdateProduct(FinishedProduct product);

    Task DeleteProduct(int id);

    Task CreateProduct(FinishedProduct product);
    int GetAmount();
}