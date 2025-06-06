using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class PositionRepository : IPositionRepository
{

    private readonly ApplicationDbContext _context;

    public PositionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Position> GetAllPositions()
    {
        return _context.Positions;
    }

    public async Task<Position> GetPositionById(int id)
    {
        return await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdatePosition(Position position)
    { 
        _context.Positions.Update(position);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExist(Position position)
    {
        return await _context.Positions.AnyAsync(x => x.Title.Equals(position.Title));
    }

    public async Task CreatePosition(Position position)
    {
        await _context.Positions.AddAsync(position);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePosition(int id)
    {
        await _context.Positions.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}