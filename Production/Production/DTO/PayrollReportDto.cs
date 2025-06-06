namespace Production.DTO;

public class PayrollReportDto
{
    public string EmployeeName { get; set; } = null!;
    public decimal? Salary { get; set; }
    public int? PurchaseParticipation { get; set; }
    public int? ProductionParticipation { get; set; }
    public int? SalesParticipation { get; set; }
    public int? TotalParticipation { get; set; }
    public decimal? Bonus { get; set; }
    public decimal? TotalAmount { get; set; }
    public string Status { get; set; } = null!;
}