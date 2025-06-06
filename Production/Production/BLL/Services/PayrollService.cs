using Production.DAL.Repositories;
using Production.DTO;

namespace Production.BLL.Services;

public class PayrollService : IPayrollService
{

    private readonly IPayrollRepository _payrollRepository;

    public PayrollService(IPayrollRepository payrollRepository)
    {
        _payrollRepository = payrollRepository;
    }

    public PayrollDateDto GetPayrollDateDto()
    {
        var model = new PayrollDateDto();
        var years = new List<int>();
        var months = new Dictionary<string, int>()
        {
            { "Январь", 1 },
            { "Февраль", 2 },
            { "Март", 3 },
            { "Апрель", 4 },
            { "Май", 5 },
            { "Июнь", 6 },
            { "Июль", 7 },
            { "Август", 8 },
            { "Сентябрь", 9 },
            { "Октябрь", 10 },
            { "Ноябрь", 11 },
            { "Декабрь", 12 }
        };
        
        int currentYear = DateTime.Now.Year;
        for (int i = currentYear - 10; i <= currentYear; i++)
        {
            years.Add(i);
        }

        model.Years = years;
        model.Months = months;

        return model;
    }

    public async Task<PayrollDto> GetPayrolls(PayrollDateDto payrollDateDto)
    {
        return await _payrollRepository.GetPayrolls(payrollDateDto);
    }

    public async Task<ServiceResponse> ProcessPayroll(decimal totalAmount, int year, int month)
    {
        var response = new ServiceResponse();
        
        var check = await _payrollRepository.CheckBudget(totalAmount);

        if (!check)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Not enough money";
            return response;
        }
        
        await _payrollRepository.ProcessPayroll(totalAmount, year, month);
        response.IsSuccess = true;

        return response;
    }

    public async Task<PayrollEditDto> GetPayrollById(int id, int year, int month)
    {
        return await _payrollRepository.GetPayrollById(id, year, month);
    }

    public async Task<bool> CheckBudget(decimal amountToPay)
    {
        return await _payrollRepository.CheckBudget(amountToPay);
    }

    public async Task UpdateTotalAmount(PayrollEditDto payrollEditDto)
    {
        await _payrollRepository.UpdateTotalAmount(payrollEditDto);
    }

    public async Task<ServiceResponse> CreatePayroll(PayrollDateDto payroll)
    {
        var response = new ServiceResponse();

        if (payroll.Year == 0 || payroll.Month == 0)
        {
            response.ErrorMessage = "Please choose proper year or month!";
            response.IsSuccess = false;
            return response;
        }

        response.IsSuccess = true;
        await _payrollRepository.CreatePayroll(payroll);
        return response;
    }
}