using Production.Models;

namespace Production.DAL.Repositories;

public interface IPositionRepository
{
    IQueryable<Position> GetAllPositions();
    Task<Position> GetPositionById(int id);
    Task UpdatePosition(Position position);
    Task<bool> IsExist(Position position);
    Task CreatePosition(Position position);
    Task DeletePosition(int id);
    
}