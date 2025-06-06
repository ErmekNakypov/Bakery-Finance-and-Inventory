using Production.DTO;

namespace Production.BLL.Services;

public interface IPayrollService
{
    PayrollDateDto GetPayrollDateDto();
    Task<ServiceResponse> CreatePayroll(PayrollDateDto payrollDateDto);
    Task<PayrollDto> GetPayrolls(PayrollDateDto payrollDateDto);
    Task<ServiceResponse> ProcessPayroll(decimal totalAmount, int year, int month);
    Task<PayrollEditDto> GetPayrollById(int id, int year, int month);
    Task<bool> CheckBudget(decimal amountToPay);
    Task UpdateTotalAmount(PayrollEditDto payrollEditDto);
}