using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Production.BLL.Services;
using Production.DTO;
using Production.Models;

namespace Production.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EmployeeController(IEmployeeService employeeService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _employeeService = employeeService;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Director")]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _employeeService.GetAll();
        var result = new List<EmployeeDto>();
        foreach (var emp in employees)
        {
            var user = emp.ApplicationUser;
            var roles = user != null ? await _userManager.GetRolesAsync(user) : new List<string>();
            var firstRole = roles.FirstOrDefault() ?? "-";

            result.Add(new EmployeeDto()
            {
                Employee = emp,
                Role = firstRole
            });
        }

        return View("Employee", result);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var model = await _employeeService.GetEmployeeById(id);
        var positions = await _roleManager.Roles.ToListAsync();
        ViewBag.Positions = positions;
        
        string currentRole = "";
        if (model.ApplicationUserId != null)
        {
            var user = await _userManager.FindByIdAsync(model.ApplicationUserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            currentRole = userRoles.FirstOrDefault() ?? "";
        }
        ViewBag.CurrentRole = currentRole;
        
        return View("EmployeeEdit", model);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEmployee(Employee employee)
    {
        if (!ModelState.IsValid)
        {
            var positions = await _roleManager.Roles.ToListAsync();
            ViewBag.Positions = positions;
        
            return View("EmployeeEdit", employee);
        }
        var selectedRole = Request.Form["SelectedRole"].ToString();
        var user = await _userManager.FindByIdAsync(employee.ApplicationUserId);
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles); 
        await _userManager.AddToRoleAsync(user, selectedRole);
        await _employeeService.UpdateEmployee(employee);
        return RedirectToAction("GetAllEmployees");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateEmployee()
    {
        var positions = await _roleManager.Roles.ToListAsync();
        ViewBag.Positions = positions;
        return View("EmployeeCreate", new EmployeeCreateDto()); 
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateEmployeePost(EmployeeCreateDto employee)
    {
        var positions = await _roleManager.Roles.ToListAsync();
        ViewBag.Positions = positions;
        if (!ModelState.IsValid)
        {
            return View("EmployeeCreate", employee);
        }

        var selectedRole = Request.Form["SelectedRole"].ToString();
        
        var email = employee.Login;
        var password = employee.Password;

        var applicationUser = await _userManager.FindByEmailAsync(email);
        if (applicationUser == null)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, selectedRole);
                var newEmp = new Employee()
                {
                    FullName = employee.FullName,
                    Address = employee.Address,
                    Phone = employee.Phone,
                    Salary = employee.Salary,
                    ApplicationUser = user
                };
                await _employeeService.CreateEmployee(newEmp);
                return RedirectToAction("GetAllEmployees");
            }
        }
        ModelState.AddModelError("", "This User Already Exists");
        return View("EmployeeCreate", employee);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var emp = await _employeeService.GetEmployeeById(id);
        await _employeeService.DeleteEmployee(id);
        var user = await _userManager.FindByIdAsync(emp.ApplicationUserId);
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, roles);
        }
        await _userManager.DeleteAsync(user);
        return RedirectToAction("GetAllEmployees");
    }
}