using Production.Models;

namespace Production.DAL.Repositories;

public interface IBudgetRepository
{
    Task<Budget> GetBudget();
    Task UpdateBudget(Budget budget);
}