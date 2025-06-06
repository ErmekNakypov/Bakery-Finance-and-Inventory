using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Production.Models;

[Table("ProductManufacturing")]
public class ProductManufacturing
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public decimal Quantity { get; set; }

    public DateOnly ManufacturingDate { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual FinishedProduct? Product { get; set; }
}
