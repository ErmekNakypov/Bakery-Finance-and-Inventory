using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Production.DTO;
using Production.Models;

namespace Production.Controllers;

public class LoginController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult InitLogin()
    {
        return View("Login", new LoginDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View("Login", model);
        }
        var result = await _signInManager.PasswordSignInAsync(
            model.Login,
            model.Password,
            false,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("GetAllProducts", "Product");
        }

        ModelState.AddModelError("", "Wrong login or password");
        return View("Login", model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return View("Login", new LoginDto());
    }
    
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View("AccessDenied");
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View("ChangePassword");
    }
    
    [HttpPost]
    public async Task<IActionResult> ChangePasswordPost(ChangePasswordDto passwordDto)
    {
        if (!ModelState.IsValid)
        {
            return View("ChangePassword", passwordDto);
        }
        
        var user = await _userManager.FindByNameAsync(User.Identity?.Name);
        var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("ChangePassword", passwordDto);
        }
        await _userManager.ChangePasswordAsync(user, user.PasswordHash, passwordDto.Password);

        return InitLogin();
    }
}