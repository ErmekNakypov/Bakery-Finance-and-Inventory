using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class CreditController : Controller
{

    private readonly ICreditService _creditService;

    public CreditController(ICreditService creditService)
    {
        _creditService = creditService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Accountant, Director")]
    public async Task<IActionResult> GetCredits()
    {
        var credits = await _creditService.GetCredits();
        return View("Credit", credits);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Accountant")]
    public IActionResult CreateCredit()
    {
        return View("CreditCreate", new Credit()); 
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> CreateCreditPost(Credit credit)
    {
        if (!ModelState.IsValid)
        {
            return View("CreditCreate", credit);
        }

        await _creditService.CreateCredit(credit);
        return RedirectToAction("GetCredits");
    }
}