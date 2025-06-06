using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public class ProductSalesService : IProductSalesService
{

    private readonly IProductSalesRepository _salesRepository;
    private readonly IBudgetService _budgetService;
    private readonly IProductService _productService;

    public ProductSalesService(IProductSalesRepository salesRepository, IBudgetService budgetService, IProductService productService)
    {
        _salesRepository = salesRepository;
        _budgetService = budgetService;
        _productService = productService;
    }

    public async Task<List<ProductSale>> GetAllProductSales()
    {
        return await _salesRepository.GetAll().ToListAsync();
    }

    public async Task<ProductSale> GetProductSaleById(int id)
    {
        return await _salesRepository.GetProductSaleById(id);
    }

    public async Task DeleteProductSale(int id)
    {
        await _salesRepository.DeleteProductSale(id);
    }

    public async Task<ServiceResponse> UpdateProductSale(ProductSale productSale)
    {
        var response = new ServiceResponse();
        if (productSale.Quantity <= 0)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Quantity must be greater than zero";
            return response;
        }
        
        var product = await _productService.GetProductById(productSale.ProductId);
        if (product.Quantity < productSale.Quantity)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Not enough quantity";
            return response;
        }

        product.Quantity -= productSale.Quantity;
        product.TotalAmount -= productSale.TotalAmount;
        await _productService.UpdateProduct(product);

        var budget = await _budgetService.GetBudget();
        budget.TotalAmount += productSale.NewPrice;
        await _budgetService.UpdateBudget(budget);
        
        productSale.TotalAmount = productSale.NewPrice;
        await _salesRepository.UpdateProductSale(productSale);
        response.IsSuccess = true;
        return response;
    }

    public async Task<ServiceResponse> CreateProductSale(ProductSale productSale)
    {
        var response = new ServiceResponse();

        if (productSale.Quantity <= 0)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Quantity must be greater than zero";
            return response;
        }
        
        var product = await _productService.GetProductById(productSale.ProductId);
        if (product.Quantity < productSale.Quantity)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Not enough quantity";
            return response;
        }

        var budget = await _budgetService.GetBudget();
        product.Quantity -= productSale.Quantity;
        product.TotalAmount -= productSale.TotalAmount;
        await _productService.UpdateProduct(product);
        
        budget.TotalAmount += productSale.NewPrice;
        await _budgetService.UpdateBudget(budget);

        productSale.TotalAmount = productSale.NewPrice;
        await _salesRepository.CreateProductSale(productSale);
        response.IsSuccess = true;
        return response;
    }

    public async Task<JsonResult> CalculateTotalAmount(decimal quantity, int productId)
    {
        var product = await _productService.GetProductById(productId);
        var price = product.TotalAmount / product.Quantity;
        var budget = await _budgetService.GetBudget();
        var addition = budget.Percentage / 100 * price * quantity;
        var totalAmount = quantity * price;
        var newPrice = totalAmount + addition;
        return new JsonResult(new { newPrice, totalAmount });
    }
}