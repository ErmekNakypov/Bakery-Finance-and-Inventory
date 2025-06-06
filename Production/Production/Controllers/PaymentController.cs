using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
namespace Production.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Accountant, Director")]
    public async Task<IActionResult> GetPayments()
    {
        var payments = await _paymentService.GetPayments();
        return View("Payment", payments);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> UpdatePaymentPenalty(int id, DateTime CalculationDate)
    {
        await _paymentService.UpdatePaymentPenalty(id, CalculationDate);
        var payments = await _paymentService.GetPayments();
        return View("Payment", payments);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Accountant")]
    public async Task<IActionResult> MakePayment(int PaymentID)
    {
        var response = await _paymentService.MakePayment(PaymentID);
        if (!response.IsSuccess)
        {
            ModelState.AddModelError("", response.ErrorMessage);
        }
        var payments = await _paymentService.GetPayments();
        return View("Payment", payments);
    }
}