using Production.Models;

namespace Production.DTO;

public class PaymentResultDto
{
    public List<Payment> Payments { get; set; } = new List<Payment>();
}