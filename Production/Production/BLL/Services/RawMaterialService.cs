using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.Models;

namespace Production.BLL.Services;

public class RawMaterialService : IRawMaterialService
{
    private readonly IRawMaterialRepository _materialRepository;

    public RawMaterialService(IRawMaterialRepository materialRepository)
    {
        _materialRepository = materialRepository;
    }

    public async Task<List<RawMaterial>> GetAll()
    {
        return await _materialRepository.GetAll().ToListAsync();
    }

    public async Task<RawMaterial> GetRawMaterialById(int id)
    {
        return await _materialRepository.GetRawMaterialById(id);
    }

    public async Task UpdateRawMaterial(RawMaterial rawMaterial)
    {
        await _materialRepository.UpdateRawMaterial(rawMaterial);
    }

    public async Task CreateRawMaterial(RawMaterial rawMaterial)
    {
        await _materialRepository.CreateRawMaterial(rawMaterial);
    }

    public async Task DeleteRawMaterial(int id)
    {
        await _materialRepository.DeleteRawMaterial(id);
    }
}