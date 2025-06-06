using Production.Models;

namespace Production.DAL.Repositories;

public interface IProductSalesRepository
{
    IQueryable<ProductSale> GetAll();
    Task<ProductSale> GetProductSaleById(int id);
    Task UpdateProductSale(ProductSale productSale);
    Task DeleteProductSale(int id);
    Task CreateProductSale(ProductSale productSale);
}