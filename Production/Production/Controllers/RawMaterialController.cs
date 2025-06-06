using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class RawMaterialController : Controller
{
    private readonly IRawMaterialService _materialService;
    private readonly IMeasurementUnitService _measurementUnitService;

    public RawMaterialController(IRawMaterialService materialService, IMeasurementUnitService measurementUnitService)
    {
        _materialService = materialService;
        _measurementUnitService = measurementUnitService;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Director, Manager")]
    public async Task<IActionResult> GetAllRawMaterials()
    {
        var model = await _materialService.GetAll();
        
        return View("RawMaterial", model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Manager")]
    public async Task<IActionResult> GetRawMaterialById(int id)
    {
        var model = await _materialService.GetRawMaterialById(id);

        var measurementUnits = await _measurementUnitService.GetAll();

        ViewBag.MeasurementUnits = measurementUnits;

        return View("RawMaterialEdit", model);
    }

    [HttpPost] 
    [Authorize(Roles = "Admin, Technologist, Manager")]
    public async Task<IActionResult> UpdateRawMaterial(RawMaterial material)
    {
        if (!ModelState.IsValid)
        {
             var measurementUnits = await _measurementUnitService.GetAll();
             ViewBag.MeasurementUnits = measurementUnits;
             return View("RawMaterialEdit", material);
        }
        await _materialService.UpdateRawMaterial(material);
        return RedirectToAction("GetAllRawMaterials");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Manager")]
    public async Task<IActionResult> CreateRawMaterial()
    {
        var measurementUnits = await _measurementUnitService.GetAll();
        ViewBag.MeasurementUnits = measurementUnits;
        
        return View("RawMaterialCreate", new RawMaterial()); 
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Technologist, Manager")]
    public async Task<IActionResult> CreateRawMaterialPost(RawMaterial material)
    {
        if (!ModelState.IsValid)
        {
            var measurementUnits = await _measurementUnitService.GetAll();
            ViewBag.MeasurementUnits = measurementUnits;
            return View("RawMaterialCreate", material);
        }
        await _materialService.CreateRawMaterial(material);
        return RedirectToAction("GetAllRawMaterials");
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Technologist, Manager")]
    public async Task<IActionResult> DeleteRawMaterial(int id)
    {
        await _materialService.DeleteRawMaterial(id);
        return RedirectToAction("GetAllRawMaterials");
    }
}