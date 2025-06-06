using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public interface IRawMaterialPurchaseService
{
    Task<List<RawMaterialPurchase>> GetAll();
    Task<RawMaterialPurchase> GetRawMaterialPurchaseById(int id);
    Task<ServiceResponse> UpdateRawMaterialPurchase(RawMaterialPurchase rawMaterialPurchase);
    Task DeleteRawMaterialPurchase(int id);
    Task<ServiceResponse> CreateRawMaterialPurchase(RawMaterialPurchase purchase);
}