namespace Production.Models;

public class Payroll
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }
    public string FullName { get; set; }
    public decimal Salary { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int PurchaseParticipation { get; set; }
    public int ProductionParticipation { get; set; }
    public int SalesParticipation { get; set; }
    public int TotalParticipation { get; set; }
    public decimal Bonus { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
}