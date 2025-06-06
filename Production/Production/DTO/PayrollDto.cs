using Production.Models;

namespace Production.DTO;

public class PayrollDto
{
    public List<Payroll> Payrolls { get; set; } = new List<Payroll>();
    public decimal TotalSum { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}