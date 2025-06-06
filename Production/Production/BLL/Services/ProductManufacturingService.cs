using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public class ProductManufacturingService : IProductManufacturingService
{
    private readonly IProductManufacturingRepository _manufacturingRepository;
    
    public ProductManufacturingService(IProductManufacturingRepository manufacturingRepository, IRawMaterialService materialService, IProductService productService, IIngredientService ingredientService)
    {
        _manufacturingRepository = manufacturingRepository;
    }

    public async Task<List<ProductManufacturing>> GetAll()
    {
        return await _manufacturingRepository.GetAll().ToListAsync();
    }

    public async Task<ProductManufacturing> GetProductManufacturingById(int id)
    {
        return await _manufacturingRepository.GetProductManufacturingById(id);
    }

    public async Task<ServiceResponse> UpdateProductManufacturing(ProductManufacturing productManufacturing)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteProductManufacturing(int id)
    {
        await _manufacturingRepository.DeleteProductManufacturing(id);
    }

    public async Task<ServiceResponse> CreateProductManufacturing(ProductManufacturing productManufacturing)
    {
        var response = new ServiceResponse();
        var isEnough = await CheckRawMaterialsAmount(productManufacturing.ProductId, productManufacturing.Quantity);

        if (!isEnough)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Not enough raw materials!";
            return response;
        }

        await _manufacturingRepository.CreateProductManufacturing(productManufacturing);
        response.IsSuccess = true;
        return response;
    }

    public async Task<bool> CheckRawMaterialsAmount(int productId, decimal quantity)
    {
        return await _manufacturingRepository.CheckRawMaterialsAmount(productId, quantity);
    }
}