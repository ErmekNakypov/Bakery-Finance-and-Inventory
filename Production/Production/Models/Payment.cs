namespace Production.Models;

public class Payment
{
    public int Id { get; set; }
    public int CreditId { get; set; }
    public int MonthNumber { get; set; }
    public DateOnly PaymentDate { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal RemainingInterest { get; set; }
    public int OverdueDays { get; set; }
    public decimal Penalty { get; set; } = 0; 
    public decimal FinalAmount { get; set; }
    public string Status { get; set; }
    
}