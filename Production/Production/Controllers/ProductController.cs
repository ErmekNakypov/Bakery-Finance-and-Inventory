using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IMeasurementUnitService _measurementUnitService;
    
    public ProductController(IProductService productService, IMeasurementUnitService measurementUnitService)
    {
        _productService = productService;
        _measurementUnitService = measurementUnitService;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Director")]
    public async Task<IActionResult> GetAllProducts()
    {
        var model = await _productService.GetAllProducts();
        
        return View("Products", model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var model = await _productService.GetProductById(id);

        var measurementUnits = await _measurementUnitService.GetAll();

        ViewBag.MeasurementUnits = measurementUnits;

        return View("ProductsEdit", model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> UpdateProduct(FinishedProduct product)
    {
        if (!ModelState.IsValid)
        {
            var measurementUnits = await _measurementUnitService.GetAll();
            ViewBag.MeasurementUnits = measurementUnits;
            return View("ProductsEdit", product);
        }
        
        await _productService.UpdateProduct(product);
        return RedirectToAction("GetAllProducts");
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProduct(id);
        return RedirectToAction("GetAllProducts");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> CreateProduct()
    {
        var measurementUnits = await _measurementUnitService.GetAll();
        ViewBag.MeasurementUnits = measurementUnits;
        
        return View("ProductsCreate", new FinishedProduct()); 
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> CreateProductPost(FinishedProduct product)
    {
        if (!ModelState.IsValid)
        {
            var measurementUnits = await _measurementUnitService.GetAll();
            ViewBag.MeasurementUnits = measurementUnits;
            return View("ProductsCreate", product);
        }
        await _productService.CreateProduct(product);
        return RedirectToAction("GetAllProducts");
    }
}   
