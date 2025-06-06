using System.ComponentModel.DataAnnotations;

namespace Production.Models;

public class Credit
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Total Amount is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Total Amount cannot be negative.")]
    public decimal TotalAmount { get; set; }
    
    public DateOnly StartDate { get; set; }
    
    [Required(ErrorMessage = "Term in years is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Term in years cannot be negative.")]
    public int TermInYears { get; set; }
    
    [Required(ErrorMessage = "InterestRate in years is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "InterestRate cannot be negative.")]
    public decimal InterestRate { get; set; }
    
    [Required(ErrorMessage = "Penalty in years is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Penalty cannot be negative.")]
    public decimal Penalty { get; set; } = 0;
}