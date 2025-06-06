using Production.DTO;

namespace Production.BLL.Services;

public interface IPaymentService
{
    Task<PaymentResultDto> GetPayments();
    Task UpdatePaymentPenalty(int id, DateTime CalculationDate);
    Task<ServiceResponse>  MakePayment(int PaymentID);
}