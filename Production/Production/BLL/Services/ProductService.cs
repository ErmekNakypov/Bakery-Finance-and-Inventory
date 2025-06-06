using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.Models;

namespace Production.BLL.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<FinishedProduct>> GetAllProducts()
    {
        return await _repository.GetAllProducts().ToListAsync();
    }

    public async Task<FinishedProduct> GetProductById(int id)
    {
        return await _repository.GetProductById(id);
    }

    public async Task UpdateProduct(FinishedProduct product)
    {
        await _repository.UpdateProduct(product);
    }

    public async Task DeleteProduct(int id)
    {
        await _repository.DeleteProduct(id);
    }

    public async Task CreateProduct(FinishedProduct product)
    {
        await _repository.CreateProduct(product);
    }

    public int GetAmount()
    {
        return _repository.GetAmount();
    }
}