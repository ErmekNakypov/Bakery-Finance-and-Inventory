using Production.DTO;
using Production.Models;

namespace Production.BLL.Services;

public interface IPositionService
{
    Task<List<Position>> GetAllPositions();
    Task<Position> GetPositionById(int id);
    Task<ServiceResponse> UpdatePosition(Position position);
    Task<ServiceResponse> CreatePosition(Position position);
    Task DeletePosition(int id);
}