using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.BLL.Services;
using Production.Models;

namespace Production.Controllers;

public class MeasurementUnitController : Controller
{
    private readonly IMeasurementUnitService _unitService;

    public MeasurementUnitController(IMeasurementUnitService unitService)
    {
        _unitService = unitService;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist, Director")]
    public async Task<IActionResult> GetAllMeasurementUnits()
    {
        var measurementUnits = await _unitService.GetAll();

        return View("MeasurementUnit", measurementUnits);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> GetMeasurementUnitById(int id)
    {
        var model = await _unitService.GetMeasurementUnitById(id);
        
        return View("MeasurementUnitEdit", model);
    }
    
    [HttpPost] 
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> UpdateMeasurementUnit(MeasurementUnit measurementUnit)
    {
        var response = await _unitService.UpdateMeasurementUnit(measurementUnit);
        if (response.IsSuccess) 
            return RedirectToAction("GetAllMeasurementUnits");
        
        ModelState.AddModelError("", response.ErrorMessage);
        return View("MeasurementUnitEdit", measurementUnit);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Technologist")]
    public IActionResult CreateMeasurementUnit()
    {
        return View("MeasurementUnitCreate", new MeasurementUnit()); 
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> CreateMeasurementUnitPost(MeasurementUnit measurementUnit)
    {
        var response = await _unitService.CreateMeasurementUnit(measurementUnit);
        if (response.IsSuccess)
        {
            return RedirectToAction("GetAllMeasurementUnits");
        }
        
        ModelState.AddModelError("", response.ErrorMessage);
        return View("MeasurementUnitCreate", measurementUnit);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, Technologist")]
    public async Task<IActionResult> DeleteMeasurementUnit(int id)
    {
        await _unitService.DeleteMeasurementUnit(id);
        return RedirectToAction("GetAllMeasurementUnits");
    }
}