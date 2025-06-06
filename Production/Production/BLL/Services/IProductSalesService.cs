using Microsoft.AspNetCore.Mvc;
using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public interface IProductSalesService
{
    Task<List<ProductSale>> GetAllProductSales();
    Task<ProductSale> GetProductSaleById(int id);
    Task DeleteProductSale(int id);
    Task<ServiceResponse> UpdateProductSale(ProductSale productSale);
    Task<ServiceResponse> CreateProductSale(ProductSale productSale);
    Task<JsonResult> CalculateTotalAmount(decimal quantity, int productId);
}