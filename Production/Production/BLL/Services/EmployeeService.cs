using Microsoft.EntityFrameworkCore;
using Production.DAL.Repositories;
using Production.Models;

namespace Production.BLL.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<List<Employee>> GetAll()
    {
        return await _employeeRepository.GetAll().ToListAsync();
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        return await _employeeRepository.GetEmployeeById(id);
    }

    public async Task UpdateEmployee(Employee employee)
    {
        await _employeeRepository.UpdateEmployee(employee);
    }

    public async Task CreateEmployee(Employee employee)
    {
        await _employeeRepository.CreateEmployee(employee);
    }

    public async Task DeleteEmployee(int id)
    {
        await _employeeRepository.DeleteEmployee(id);
    }
}