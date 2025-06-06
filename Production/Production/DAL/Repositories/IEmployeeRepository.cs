using Production.Models;

namespace Production.DAL.Repositories;

public interface IEmployeeRepository
{
    IQueryable<Employee> GetAll();
    Task<Employee> GetEmployeeById(int id);
    Task UpdateEmployee(Employee employee);
    
    Task CreateEmployee(Employee employee);
    Task DeleteEmployee(int id);
}