using Production.Models;

namespace Production.DAL.Repositories;

public interface IRawMaterialRepository
{
    IQueryable<RawMaterial> GetAll();
    
    Task<RawMaterial> GetRawMaterialById(int id);

    Task UpdateRawMaterial(RawMaterial rawMaterial);
    
    Task CreateRawMaterial(RawMaterial rawMaterial);

    Task DeleteRawMaterial(int id);
}