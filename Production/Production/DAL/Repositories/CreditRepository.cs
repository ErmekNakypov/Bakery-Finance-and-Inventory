using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class CreditRepository : ICreditRepository
{

    private readonly ApplicationDbContext _context;

    public CreditRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Credit>> GetCredits()
    {
        var credits = await _context.Credits
            .FromSql($"EXEC GetCredits")
            .ToListAsync();

        return credits;
    }

    public async Task CreateCredit(Credit credit)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($@"
        EXEC CreateCredit 
            @TotalAmount = {credit.TotalAmount}, 
            @StartDate = {credit.StartDate}, 
            @TermInYears = {credit.TermInYears}, 
            @InterestRate = {credit.InterestRate}, 
            @Penalty = {credit.Penalty}");
    }
}