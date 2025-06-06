using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Production.Controllers;

public class PositionController : Controller
{
    
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public PositionController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Director")]
    public async Task<IActionResult> GetAllPositions()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        return View("Position", roles);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPositionById(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        
        return View("PositionEdit", role);
    }
    
    [HttpPost] 
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePosition(IdentityRole role)
    {
        var existingRole = await _roleManager.FindByIdAsync(role.Id);
        if (existingRole == null)
            return NotFound();

        existingRole.Name = role.Name;
        existingRole.NormalizedName = role.Name.ToUpper();

        var result = await _roleManager.UpdateAsync(existingRole);
        if (result.Succeeded)
            return RedirectToAction("GetAllPositions");

        ModelState.AddModelError("", "Could not update role");
        return View("PositionEdit", role);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult CreatePosition()
    {
        return View("PositionCreate", new IdentityRole()); 
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePositionPost(IdentityRole role)
    {
        if (!ModelState.IsValid)
            return View("PositionCreate", role);

        var result = await _roleManager.CreateAsync(new IdentityRole(role.Name));
        if (result.Succeeded)
            return RedirectToAction("GetAllPositions");

        ModelState.AddModelError("", "Could not create role");
        return View("PositionCreate", role);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePosition(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
            await _roleManager.DeleteAsync(role);

        return RedirectToAction("GetAllPositions");
    }
}