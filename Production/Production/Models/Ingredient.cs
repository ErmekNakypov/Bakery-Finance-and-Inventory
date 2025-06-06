using System.ComponentModel.DataAnnotations;
namespace Production.Models;
public class Ingredient
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int RawMaterialId { get; set; }
    
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public decimal Quantity { get; set; }
    public virtual FinishedProduct? Product { get; set; }
    public virtual RawMaterial? RawMaterial { get; set; }
}
