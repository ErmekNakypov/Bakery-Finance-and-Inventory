using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class BudgetController : Controller
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Director, Accountant, Manager")]
    public async Task<IActionResult> GetBudget()
    {
        var budget = await _budgetService.GetBudget();

        return View("Budget", budget);
    }
    
    [HttpPost] 
    [Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> UpdateBudget(Budget budget)
    {
        if (!ModelState.IsValid)
        {
            return View("BudgetEdit", budget);
        }

        await _budgetService.UpdateBudget(budget);
        return View("Budget", budget);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> GetBudgetForUpdate()
    {
        var budget = await _budgetService.GetBudget();
        return View("BudgetEdit", budget);
    }
}