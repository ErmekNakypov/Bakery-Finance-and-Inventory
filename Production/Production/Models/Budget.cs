using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Production.Models;

[Table("Budget")]
public class Budget
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Total Amount is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Total Amount cannot be negative.")]
    public decimal TotalAmount { get; set; }
    
    [Required(ErrorMessage = "Percentage is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Percentage cannot be negative.")]
    public decimal Percentage { get; set; }
    
    [Required(ErrorMessage = "Percentage is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Percentage cannot be negative.")]
    public decimal BonusPercentage { get; set; }
}
