using System.ComponentModel.DataAnnotations;

namespace Production.DTO;

public class PayrollEditDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    
    [Required(ErrorMessage = "Total Amount is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Total Amount cannot be negative.")]
    public decimal TotalAmount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}