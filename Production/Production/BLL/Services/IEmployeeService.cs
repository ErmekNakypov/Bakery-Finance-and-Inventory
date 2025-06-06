using Production.Models;

namespace Production.BLL.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAll();
    Task<Employee> GetEmployeeById(int id);
    Task UpdateEmployee(Employee employee);
    Task CreateEmployee(Employee employee);
    Task DeleteEmployee(int id);
}