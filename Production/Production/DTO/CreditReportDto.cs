using Production.Models;

namespace Production.DTO;

public class CreditReportDto
{
    public DateOnly PaymentDate { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal RemainingInterest { get; set; }
    public int OverdueDays { get; set; }
    public decimal Penalty { get; set; } = 0;
    public decimal FinalAmount { get; set; }
    public string Status { get; set; }
    public int CreditId { get; set; }
    public Credit? Credit { get; set; }
}