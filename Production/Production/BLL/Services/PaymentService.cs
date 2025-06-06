using Production.DAL.Repositories;
using Production.DTO;

namespace Production.BLL.Services;
public class PaymentService : IPaymentService
{

    private readonly IPaymentRepository _paymentRepository;
    private readonly IPayrollRepository _payrollRepository;

    public PaymentService(IPaymentRepository paymentRepository, IPayrollRepository payrollRepository)
    {
        _paymentRepository = paymentRepository;
        _payrollRepository = payrollRepository;
    }

    public async Task<PaymentResultDto> GetPayments()
    {
        return await _paymentRepository.GetPayments();
    }

    public async Task UpdatePaymentPenalty(int id, DateTime CalculationDate)
    {
        await _paymentRepository.UpdatePaymentPenalty(id, CalculationDate);
    }

    public async Task<ServiceResponse> MakePayment(int PaymentID)
    {
        var response = new ServiceResponse();
        var payment = await _paymentRepository.GetPaymentById(PaymentID);

        var check = await _payrollRepository.CheckBudget(payment.FinalAmount);
        if (!check)
        {
            response.IsSuccess = false;
            response.ErrorMessage = "Not enough money";
            return response;
        }
        response.IsSuccess = true;
        await _paymentRepository.MakePayment(PaymentID);
        return response;
    }
}