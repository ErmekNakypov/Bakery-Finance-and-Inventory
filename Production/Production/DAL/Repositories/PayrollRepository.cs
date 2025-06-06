using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.DTO;


namespace Production.DAL.Repositories;

public class PayrollRepository : IPayrollRepository
{

    private readonly ApplicationDbContext _context;

    public PayrollRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreatePayroll(PayrollDateDto payrollDateDto)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC CalculatePayroll @Year, @Month",
            new SqlParameter("@Year", payrollDateDto.Year),
            new SqlParameter("@Month", payrollDateDto.Month)
        );
    }

    public async Task<PayrollDto> GetPayrolls(PayrollDateDto payrollDateDto)
    {
        
        var yearParam = new SqlParameter("@Year", payrollDateDto.Year);
        var monthParam = new SqlParameter("@Month", payrollDateDto.Month);
        var totalSumParam = new SqlParameter
        {
            ParameterName = "@TotalSum",
            SqlDbType = SqlDbType.Decimal,
            Direction = ParameterDirection.Output
        };
        
        var payrolls = await _context.Payrolls
            .FromSqlInterpolated($"EXEC GetPayrolls @Year={yearParam}, @Month={monthParam}, @TotalSum={totalSumParam} OUTPUT")
            .ToListAsync();

        var payrollData = new PayrollDto()
        {
            Payrolls = payrolls,
            TotalSum = totalSumParam.Value != DBNull.Value ? (decimal)totalSumParam.Value : 0m,
            Year = payrollDateDto.Year,
            Month = payrollDateDto.Month
        };
        
        return payrollData;
    }

    public async Task ProcessPayroll(decimal totalAmount, int year, int month)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC ProcessPayroll @Year, @Month, @TotalAmount",
            new SqlParameter("@Year", year),
            new SqlParameter("@Month", month),
            new SqlParameter("@TotalAmount", totalAmount));
    }

    public async Task<PayrollEditDto> GetPayrollById(int id, int year, int month)
    {
        var payrollData = await _context.Database
            .SqlQueryRaw<PayrollSingleEditDto>("EXEC GetPayrollById @PayrollID = {0}", id)
            .ToListAsync();

        var temp = payrollData.FirstOrDefault();
        var res = new PayrollEditDto()
        {
            Id = temp.Id,
            FullName = temp.FullName,
            TotalAmount = temp.TotalAmount,
            Year = year,
            Month = month
        };
        
        return res;
    }

    public async Task<bool> CheckBudget(decimal amountToPay)
    {
        var isEnoughParam = new SqlParameter
        {
            ParameterName = "@IsEnough",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC CheckBudget @AmountToPay, @IsEnough OUTPUT",
            new SqlParameter("@AmountToPay", amountToPay),
            isEnoughParam
        );

        return (bool)isEnoughParam.Value;
    }

    public async Task UpdateTotalAmount(PayrollEditDto payrollEditDto)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC UpdatePayrollTotalAmount @ID, @TotalAmount",
            new SqlParameter("@ID", payrollEditDto.Id),
            new SqlParameter("@TotalAmount", payrollEditDto.TotalAmount)
        );
    }
}