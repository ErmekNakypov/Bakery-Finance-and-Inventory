using Production.DTO;

namespace Production.DAL.Repositories;

public interface IPayrollRepository
{
    Task CreatePayroll(PayrollDateDto payrollDateDto);
    Task<PayrollDto> GetPayrolls(PayrollDateDto payrollDateDto);
    Task ProcessPayroll(decimal totalAmount, int year, int month);
    Task<PayrollEditDto> GetPayrollById(int id, int year, int month);
    Task<Boolean> CheckBudget(decimal amountToPay);
    Task UpdateTotalAmount(PayrollEditDto payrollEditDto);
}