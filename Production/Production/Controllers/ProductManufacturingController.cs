using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class ProductManufacturingController : Controller
{
    private readonly IProductManufacturingService _manufacturingService;
    private readonly IProductService _productService;
    private readonly IEmployeeService _employeeService;

    public ProductManufacturingController(IProductManufacturingService manufacturingService, IProductService productService, IEmployeeService employeeService)
    {
        _manufacturingService = manufacturingService;
        _productService = productService;
        _employeeService = employeeService;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Director")]
    public async Task<IActionResult> GetAllProductManufacturing()
    {
        var model = await _manufacturingService.GetAll();
        return View("ProductManufacturing", model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> GetProductManufacturingId(int id)
    {
        var model = await _manufacturingService.GetProductManufacturingById(id);
        var products = await _productService.GetAllProducts();
        ViewBag.Products = products;
        var employees = await _employeeService.GetAll();
        ViewBag.Employees = employees;

        return View("ProductManufacturingEdit", model);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> DeleteProductManufacturing(int id)
    {
        await _manufacturingService.DeleteProductManufacturing(id);
        return RedirectToAction("GetAllProductManufacturing");
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> CreateProductManufacturing()
    {
        var products = await _productService.GetAllProducts();
        ViewBag.Products = products;

        var employees = await _employeeService.GetAll();
        ViewBag.Employees = employees;
        
        return View("ProductManufacturingCreate", new ProductManufacturing()); 
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> CreateProductManufacturingPost(ProductManufacturing productManufacturing)
    {
        var response = await _manufacturingService.CreateProductManufacturing(productManufacturing);
        if (!ModelState.IsValid || !response.IsSuccess)
        {
            ModelState.AddModelError("", response.ErrorMessage);
            var products = await _productService.GetAllProducts();
            ViewBag.Products = products;

            var employees = await _employeeService.GetAll();
            ViewBag.Employees = employees;
            return View("ProductManufacturingCreate", productManufacturing);
        }
        
        return RedirectToAction("GetAllProductManufacturing");
    }
    
    [HttpPost] 
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> UpdateProductManufacturing(ProductManufacturing productManufacturing)
    {
        var response = await _manufacturingService.UpdateProductManufacturing(productManufacturing);
        if (!ModelState.IsValid && !response.IsSuccess)
        {
            ModelState.AddModelError("", response.ErrorMessage);
            var products = await _productService.GetAllProducts();
            ViewBag.Products = products;

            var employees = await _employeeService.GetAll();
            ViewBag.Employees = employees;
            return View("ProductManufacturingEdit", productManufacturing);
        }
     
        return RedirectToAction("GetAllProductManufacturing");
    }
}