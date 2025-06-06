using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Production.BLL.Services;
using Production.DAL.EntityFramework;
using Production.DTO;
using Production.Models;

namespace Production.Controllers;

public class ProfileController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ProfileController(IEmployeeService employeeService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _employeeService = employeeService;
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = _userManager.GetUserId(User);
        var employee = await _context.Employees
            .Include(e => e.ApplicationUser)
            .FirstOrDefaultAsync(e => e.ApplicationUserId == userId);

        var employeeDto = new UpdateEmployeeDto()
        {
            FullName = employee.FullName,
            Address = employee.Address,
            Phone = employee.Phone
        };
        
        return View("Profile", employeeDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateEmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            return View("Profile", employeeDto); 
        }
        var userId = _userManager.GetUserId(User);
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == userId);

        if (employee == null)
        {
            return NotFound();
        }
        
        employee.FullName = employeeDto.FullName;
        employee.Address = employeeDto.Address;
        employee.Phone = employeeDto.Phone;

        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();

        return RedirectToAction("GetProfile");
    }
}