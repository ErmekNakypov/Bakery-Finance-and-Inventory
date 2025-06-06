using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class ProductSaleController : Controller
{
    private readonly IProductSalesService _salesService;
    private readonly IProductService _productService;
    private readonly IEmployeeService _employeeService;

    public ProductSaleController(IProductSalesService salesService, IProductService productService, IEmployeeService employeeService)
    {
        _salesService = salesService;
        _productService = productService;
        _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Director")]
    public async Task<IActionResult> GetAllProductSales()
    {
        var model = await _salesService.GetAllProductSales();
        return View("ProductSale", model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> GetProductSaleById(int id)
    {
        var model = await _salesService.GetProductSaleById(id);
        var products = await _productService.GetAllProducts();
        ViewBag.Products = products;
        var employees = await _employeeService.GetAll();
        ViewBag.Employees = employees;

        return View("ProductSaleEdit", model);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> DeleteProductSale(int id)
    {
        await _salesService.DeleteProductSale(id);
        return RedirectToAction("GetAllProductSales");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateProductSale()
    {
        var products = await _productService.GetAllProducts();
        ViewBag.Products = products;

        var employees = await _employeeService.GetAll();
        ViewBag.Employees = employees;
        
        return View("ProductSaleCreate", new ProductSale()); 
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateProductSalePost(ProductSale productSale)
    {
        var response = await _salesService.CreateProductSale(productSale);
        if (!ModelState.IsValid || !response.IsSuccess)
        {
            ModelState.AddModelError("", response.ErrorMessage);
            var products = await _productService.GetAllProducts();
            ViewBag.Products = products;

            var employees = await _employeeService.GetAll();
            ViewBag.Employees = employees;
            
            return View("ProductSaleCreate", productSale);
        }
        
        return RedirectToAction("GetAllProductSales");
    }
    
    [HttpPost] 
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> UpdateProductSale(ProductSale productSale)
    {
        var response = await _salesService.UpdateProductSale(productSale);
        if (!ModelState.IsValid && !response.IsSuccess)
        {
            ModelState.AddModelError("", response.ErrorMessage);
            var products = await _productService.GetAllProducts();
            ViewBag.Products = products;

            var employees = await _employeeService.GetAll();
            ViewBag.Employees = employees;
            return View("ProductSaleEdit", productSale);
        }
     
        return RedirectToAction("GetAllProductSales");
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<JsonResult> CalculateTotalAmount(int productId, int quantity)
    {
        var newPriceJsonResult = await _salesService.CalculateTotalAmount(quantity, productId);

        return newPriceJsonResult;
    }
}