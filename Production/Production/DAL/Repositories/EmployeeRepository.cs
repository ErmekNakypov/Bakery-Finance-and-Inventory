using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.Models;

namespace Production.DAL.Repositories;

public class EmployeeRepository : IEmployeeRepository
{

    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Employee> GetAll()
    {
        return _context.Employees.Include(x => x.ApplicationUser);
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        return await _context.Employees.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateEmployee(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task CreateEmployee(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmployee(int id)
    {
        await _context.Employees.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}