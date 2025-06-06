namespace Production.DTO;

public class CreditTotalReportDto
{
    public decimal? PrincipalAmounts { get; set; }
    public decimal? InterestAmounts { get; set; }
    public decimal? TotalAmounts { get; set; }
    public decimal? RemainingInterests { get; set; }
    public int? OverdueDays { get; set; }
    public decimal? Penalties { get; set; }
    public decimal? FinalAmounts { get; set; }
}