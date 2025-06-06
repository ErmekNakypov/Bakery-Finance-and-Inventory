using Production.Models;

namespace Production.BLL.Services;

public interface IRawMaterialService
{
    Task<List<RawMaterial>> GetAll();
    
    Task<RawMaterial> GetRawMaterialById(int id);

    Task UpdateRawMaterial(RawMaterial rawMaterial);
    
    Task CreateRawMaterial(RawMaterial rawMaterial);

    Task DeleteRawMaterial(int id);
}