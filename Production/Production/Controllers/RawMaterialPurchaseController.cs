using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class RawMaterialPurchaseController : Controller
{
    private readonly IRawMaterialPurchaseService _purchaseService;
    private readonly IRawMaterialService _materialService;
    private readonly IEmployeeService _employeeService;

    public RawMaterialPurchaseController(IRawMaterialPurchaseService purchaseService, IRawMaterialService materialService, IEmployeeService employeeService)
    {
        _purchaseService = purchaseService;
        _materialService = materialService;
        _employeeService = employeeService;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Director")]
    public async Task<IActionResult> GetAllRawMaterialPurchases()
    {
        var model = await _purchaseService.GetAll();
        
        return View("RawMaterialPurchase", model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> GetRawMaterialPurchaseById(int id)
    {
        var model = await _purchaseService.GetRawMaterialPurchaseById(id);

        var rawMaterials = await _materialService.GetAll();
        ViewBag.RawMaterials = rawMaterials;

        var employees = await _employeeService.GetAll();
        ViewBag.Employees = employees;

        return View("RawMaterialPurchaseEdit", model);
    }
    
    [HttpPost] 
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> UpdateRawMaterialPurchase(RawMaterialPurchase material)
    {
        var response = await _purchaseService.UpdateRawMaterialPurchase(material);
        if (!ModelState.IsValid && !response.IsSuccess)
        {
            ModelState.AddModelError("", response.ErrorMessage);
            var rawMaterials = await _materialService.GetAll();
            ViewBag.RawMaterials = rawMaterials;

            var employees = await _employeeService.GetAll();
            ViewBag.Employees = employees;
            return View("RawMaterialPurchaseEdit", material);
        }
        await _purchaseService.UpdateRawMaterialPurchase(material);
        return RedirectToAction("GetAllRawMaterialPurchases");
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> DeleteRawMaterialPurchase(int id)
    {
        await _purchaseService.DeleteRawMaterialPurchase(id);
        return RedirectToAction("GetAllRawMaterialPurchases");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateRawMaterialPurchase()
    {
        var rawMaterials = await _materialService.GetAll();
        ViewBag.RawMaterials = rawMaterials;

        var employees = await _employeeService.GetAll();
        ViewBag.Employees = employees;
        
        return View("RawMaterialPurchaseCreate", new RawMaterialPurchase()); 
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateRawMaterialPurchasePost(RawMaterialPurchase purchase)
    {
        var response = await _purchaseService.CreateRawMaterialPurchase(purchase);
        if (!ModelState.IsValid || !response.IsSuccess)
        {
            ModelState.AddModelError("", response.ErrorMessage);
            var rawMaterials = await _materialService.GetAll();
            ViewBag.RawMaterials = rawMaterials;

            var employees = await _employeeService.GetAll();
            ViewBag.Employees = employees;
            return View("RawMaterialPurchaseCreate", purchase);
        }
        
        return RedirectToAction("GetAllRawMaterialPurchases");
    }
}