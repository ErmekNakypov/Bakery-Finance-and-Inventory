namespace Production.DTO;

public class PayrollReportTotalDto
{
    public int? PurchaseParticipation { get; set; }
    public int? ProductionParticipation { get; set; }
    public int? SalesParticipation { get; set; }
    public int? TotalParticipation { get; set; }
    public decimal? Bonus { get; set; }
    public decimal? TotalAmount { get; set; }
}