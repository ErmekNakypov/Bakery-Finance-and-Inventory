using Production.Models;

namespace Production.BLL.Services;

public interface IBudgetService
{
    Task<Budget> GetBudget();
    Task UpdateBudget(Budget budget);
}