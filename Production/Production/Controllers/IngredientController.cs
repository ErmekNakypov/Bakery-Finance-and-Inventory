using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class IngredientController : Controller
{
    private readonly IIngredientService _ingredientService;
    private readonly IProductService _productService;
    private readonly IRawMaterialService _materialService;

    public IngredientController(IIngredientService ingredientService, IProductService productService, IRawMaterialService materialService)
    {
        _ingredientService = ingredientService;
        _productService = productService;
        _materialService = materialService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Director")]
    public async Task<IActionResult> InitIngredients()
    {
        var model = await _productService.GetAllProducts();
        ViewBag.Products = model;

        return View("Ingredient", new List<Ingredient>());
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Director")]
    public async Task<IActionResult> GetIngredientsByProductId(int id)
    {
        HttpContext.Session.SetInt32("SelectedProductId", id);
        ViewBag.Products = await _productService.GetAllProducts();
        var model = await _ingredientService.GetIngredientsByProductId(id);
        var product = await _productService.GetProductById((int)id);
        ViewBag.ProductsName = product.Name;
        return View("Ingredient", model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> GetIngredientById(int id)
    {
        var model = await _ingredientService.GetIngredientById(id);
        
        var products = await _productService.GetAllProducts();
        ViewBag.Products = products;
        var materials = await _materialService.GetAll();
        ViewBag.RawMaterials = materials;

        return View("IngredientEdit", model);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> UpdateIngredient(Ingredient ingredient)
    {
        if (!ModelState.IsValid)
        {
            var products = await _productService.GetAllProducts();
            ViewBag.Products = products;
            var materials = await _materialService.GetAll();
            ViewBag.RawMaterials = materials;
            return View("IngredientEdit", ingredient);
        }
        await _ingredientService.UpdateIngredient(ingredient);
        return Redirect($"~/Ingredient/GetIngredientsByProductId?id={HttpContext.Session.GetInt32("SelectedProductId")}");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> CreateIngredient()
    {
        var products = await _productService.GetAllProducts();
        ViewBag.Products = products;
        var materials = await _materialService.GetAll();
        ViewBag.RawMaterials = materials;
        Ingredient ingredient = new Ingredient();
        
        return View("IngredientCreate", ingredient); 
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> CreateIngredientPost(Ingredient ingredient)
    {
        
        var response = await _ingredientService.CreateIngredient(ingredient);

        if (!response.IsSuccess)
        {
            var products = await _productService.GetAllProducts();
            ViewBag.Products = products;
            var materials = await _materialService.GetAll();
            ViewBag.RawMaterials = materials;
            ModelState.AddModelError("", response.ErrorMessage);
            return View("IngredientCreate", ingredient);
        }
        
        return Redirect($"~/Ingredient/GetIngredientsByProductId?id={HttpContext.Session.GetInt32("SelectedProductId")}");
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        await _ingredientService.DeleteIngredient(id);
        return Redirect($"~/Ingredient/GetIngredientsByProductId?id={HttpContext.Session.GetInt32("SelectedProductId")}");
    }
}