using Production.DTO;
using Production.Models;

namespace Production.DAL.Repositories;

public interface IPaymentRepository
{
    Task<PaymentResultDto> GetPayments();
    Task UpdatePaymentPenalty(int id, DateTime CalculationDate);
    Task MakePayment(int PaymentID);
    Task<Payment> GetPaymentById(int id);
}