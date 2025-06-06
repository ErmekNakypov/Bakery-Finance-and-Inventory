using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public interface IProductManufacturingService
{
    Task<List<ProductManufacturing>> GetAll();
    Task<ProductManufacturing> GetProductManufacturingById(int id);
    Task<ServiceResponse> UpdateProductManufacturing(ProductManufacturing productManufacturing);
    Task DeleteProductManufacturing(int id);
    Task<ServiceResponse> CreateProductManufacturing(ProductManufacturing productManufacturing);
    Task<bool> CheckRawMaterialsAmount(int productId, decimal quantity);
}