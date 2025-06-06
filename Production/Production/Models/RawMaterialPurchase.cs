using System.ComponentModel.DataAnnotations;

namespace Production.Models;

public class RawMaterialPurchase
{
    public int Id { get; set; }

    public int RawMaterialId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public decimal Quantity { get; set; }

    [Required(ErrorMessage = "Total Amount is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Total Amount cannot be negative.")]
    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "Purchase Date is required.")]
    public DateOnly PurchaseDate { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual RawMaterial? RawMaterial { get; set; }
}
