using Production.DAL.Repositories;
using Production.Models;

namespace Production.BLL.Services;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }
    public async Task<Budget> GetBudget()
    {
        return await _budgetRepository.GetBudget();
    }

    public async Task UpdateBudget(Budget budget)
    {
        await _budgetRepository.UpdateBudget(budget);
    }
}