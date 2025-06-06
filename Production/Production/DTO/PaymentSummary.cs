namespace Production.DTO;

public class PaymentSummary
{
    public decimal TotalPrincipalAmount { get; set; }
    public decimal TotalInterestAmount { get; set; }
    public decimal TotalTotalAmount { get; set; }
    public decimal TotalRemainingInterest { get; set; }
    public decimal TotalOverdueDays { get; set; }
    public decimal TotalPenalty { get; set; }
    public decimal TotalFinalAmount { get; set; }
}