namespace Production.DTO;

public class PayrollDateDto
{
    public int Year { get; set; } = 0;
    public int Month { get; set; } = 0;
    public List<int>? Years { get; set; }
    public Dictionary<string, int>? Months { get; set; }
}