using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Production.Models;

public class ProductSale
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public decimal Quantity { get; set; }
    
    [Required(ErrorMessage = "Total Amount is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Total Amount cannot be negative.")]
    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "Sale Date is required.")]
    public DateOnly SaleDate { get; set; }
    
    public int EmployeeId { get; set; }
    
    public virtual Employee? Employee { get; set; }
    
    public virtual FinishedProduct? Product { get; set; }
    
    [NotMapped]
    public decimal NewPrice { get; set; }
}
