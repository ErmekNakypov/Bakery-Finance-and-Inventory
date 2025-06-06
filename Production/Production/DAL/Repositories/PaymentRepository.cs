using Microsoft.EntityFrameworkCore;
using Production.DAL.EntityFramework;
using Production.DTO;
using Production.Models;

namespace Production.DAL.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;
    
    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentResultDto> GetPayments()
    {
        var payments = await _context.Payments
            .FromSql($"EXEC GetPayments")
            .ToListAsync();
        
        return new PaymentResultDto()
        {
            Payments = payments,
        };
    }

    public async Task UpdatePaymentPenalty(int id, DateTime CalculationDate)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC UpdatePaymentPenalty @PaymentID = {0}, @CalculationDate = {1}",
            id, CalculationDate);
    }

    public async Task MakePayment(int PaymentID)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC MakePayment @PaymentID = {0}",
            PaymentID, 0);
    }

    public async Task<Payment> GetPaymentById(int id)
    {
        var paymentData = await _context.Database
            .SqlQueryRaw<Payment>("EXEC GetPaymentById @PaymentID = {0}", id)
            .ToListAsync();
        return paymentData.FirstOrDefault();
    }
}