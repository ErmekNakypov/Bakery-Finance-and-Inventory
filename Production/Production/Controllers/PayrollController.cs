using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.DTO;

namespace Production.Controllers;
[Authorize(Roles = "Admin, Director")]
public class PayrollController : Controller
{
    
    private readonly IPayrollService _payrollService;

    public PayrollController(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    [HttpGet]
    public IActionResult InitPayroll()
    {
        ViewBag.Payrolls = _payrollService.GetPayrollDateDto();
        return View("Payroll", new PayrollDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePayroll(PayrollDateDto payrollDateDto)
    {
        await _payrollService.CreatePayroll(payrollDateDto);
        var newPayroll = _payrollService.GetPayrollDateDto();
        ViewData["Year"] = payrollDateDto.Year;
        ViewData["Month"] = payrollDateDto.Month;
        ViewBag.Payrolls = newPayroll;
        var model = await _payrollService.GetPayrolls(payrollDateDto);
        return View("Payroll", model);
    }
    
    [HttpGet] 
    public async Task<IActionResult> ProcessPayroll(decimal totalSum, int year, int month)
    {
        var process = await _payrollService.ProcessPayroll(totalSum, year, month);

        if (!process.IsSuccess)
        {
            ModelState.AddModelError("", process.ErrorMessage);
        }
        
        var newPayroll = _payrollService.GetPayrollDateDto();
        ViewBag.Payrolls = newPayroll;
        ViewData["Year"] = year;
        ViewData["Month"] = month;
        var date = new PayrollDateDto
        {
            Month = month,
            Year = year
        };
        var model = await _payrollService.GetPayrolls(date);
        return View("Payroll", model);
    }

    [HttpGet] 
    public async Task<IActionResult> GetPayrollById(int id, int year, int month)
    {
        var res = await _payrollService.GetPayrollById(id, year, month);
        return View("PayrollEdit", res);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTotalAmount(PayrollEditDto payrollEditDto)
    {
        await _payrollService.UpdateTotalAmount(payrollEditDto);
        var newPayroll = _payrollService.GetPayrollDateDto();
        ViewBag.Payrolls = newPayroll;
        ViewData["Year"] = payrollEditDto.Year;
        ViewData["Month"] = payrollEditDto.Month;
        var date = new PayrollDateDto
        {
            Month = payrollEditDto.Month,
            Year = payrollEditDto.Year
        };
        var model = await _payrollService.GetPayrolls(date);
        return View("Payroll", model);
    }
}