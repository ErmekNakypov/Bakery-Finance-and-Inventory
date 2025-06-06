using Microsoft.AspNetCore.Identity;
using Production.DAL.Repositories;
using Production.Models;

namespace Production.DAL.Seed;

public class DbInitializer
{
    
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        string[] roles = { "Admin", "Accountant", "Director", "Technologist", "Manager"};
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = configuration["Admin:Email"];
        var adminPassword = configuration["Admin:Password"];
        
      
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new ApplicationUser
            {
                UserName = configuration["Admin:Username"],
                Email = adminEmail,
                EmailConfirmed = true
            };

            var employee = new Employee()
            {
                FullName = "Admin",
                Salary = 0,
                Address = "Bishkek",
                Phone = "+996500500500",
                ApplicationUser = user
            };
            
            var result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                var employeeRepository = serviceProvider.GetRequiredService<IEmployeeRepository>();
                await employeeRepository.CreateEmployee(employee);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}