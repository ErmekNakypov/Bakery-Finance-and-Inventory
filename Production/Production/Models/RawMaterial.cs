using System.ComponentModel.DataAnnotations;

namespace Production.Models;

public class RawMaterial
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;

    public int MeasurementUnitId { get; set; }

    
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public decimal Quantity { get; set; }

    
    [Required(ErrorMessage = "Total Amount is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Total Amount cannot be negative.")]
    public decimal TotalAmount { get; set; }

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    
    public virtual MeasurementUnit? MeasurementUnit { get; set; }

    public virtual ICollection<RawMaterialPurchase> RawMaterialPurchases { get; set; } = new List<RawMaterialPurchase>();
}
