using Production.Models;

namespace Production.DAL.Repositories;

public interface IRawMaterialPurchaseRepository
{
    IQueryable<RawMaterialPurchase> GetAll();
    Task<RawMaterialPurchase> GetRawMaterialPurchaseById(int id);
    Task UpdateRawMaterialPurchase(RawMaterialPurchase rawMaterialPurchase);
    Task DeleteRawMaterialPurchase(int id);
    Task CreateRawMaterialPurchase(RawMaterialPurchase purchase);
}