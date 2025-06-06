using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public class RawMaterialPurchaseService : IRawMaterialPurchaseService
{

    private readonly IRawMaterialPurchaseRepository _repository;
    private readonly IBudgetService _budgetService;
    private readonly IRawMaterialService _rawMaterialService;
    
    public RawMaterialPurchaseService(IRawMaterialPurchaseRepository repository, IBudgetService budgetService, IRawMaterialService rawMaterialService)
    {
        _repository = repository;
        _budgetService = budgetService;
        _rawMaterialService = rawMaterialService;
    }

    public async Task<List<RawMaterialPurchase>> GetAll()
    {
        return await _repository.GetAll().ToListAsync();
    }

    public async Task<RawMaterialPurchase> GetRawMaterialPurchaseById(int id)
    {
        return await _repository.GetRawMaterialPurchaseById(id);
    }

    public async Task<ServiceResponse> UpdateRawMaterialPurchase(RawMaterialPurchase purchase)
    {
        var response = new ServiceResponse();
        var budget = await _budgetService.GetBudget();

        if (budget.TotalAmount < purchase.TotalAmount)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Not enough money";
            return response;
        }

        var rawMaterial = await _rawMaterialService.GetRawMaterialById(purchase.RawMaterialId);
        rawMaterial.Quantity += purchase.Quantity;
        rawMaterial.TotalAmount += purchase.TotalAmount;
        budget.TotalAmount -= purchase.TotalAmount;
        await _budgetService.UpdateBudget(budget);
        await _rawMaterialService.UpdateRawMaterial(rawMaterial);
        await _repository.UpdateRawMaterialPurchase(purchase);
        
        response.IsSuccess = true;
        return response;
    }

    public async Task DeleteRawMaterialPurchase(int id)
    {
        await _repository.DeleteRawMaterialPurchase(id);
    }

    public async Task<ServiceResponse> CreateRawMaterialPurchase(RawMaterialPurchase purchase)
    {
        var response = new ServiceResponse();
        var budget = await _budgetService.GetBudget();

        if (budget.TotalAmount < purchase.TotalAmount)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Not enough money";
            return response;
        }

        var rawMaterial = await _rawMaterialService.GetRawMaterialById(purchase.RawMaterialId);
        rawMaterial.Quantity += purchase.Quantity;
        rawMaterial.TotalAmount += purchase.TotalAmount;
        budget.TotalAmount -= purchase.TotalAmount;
        await _budgetService.UpdateBudget(budget);
        await _rawMaterialService.UpdateRawMaterial(rawMaterial);
        await _repository.CreateRawMaterialPurchase(purchase);
        
        response.IsSuccess = true;
        return response;
    }
}