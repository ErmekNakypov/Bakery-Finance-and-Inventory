using System.ComponentModel.DataAnnotations;

namespace Production.Models;

public class Employee
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Fullname is required.")]
    public string FullName { get; set; } = null!;
    
    [Required(ErrorMessage = "Salary is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0.")]
    public decimal Salary { get; set; }
    
    [Required(ErrorMessage = "Address is required.")]
    public string? Address { get; set; }
    
    [Required(ErrorMessage = "Phone is required.")]
    public string? Phone { get; set; }
    
    public string? ApplicationUserId { get; set; }
    public virtual ApplicationUser? ApplicationUser { get; set; }

    public virtual ICollection<ProductManufacturing> ProductManufacturings { get; set; } = new List<ProductManufacturing>();

    public virtual ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();

    public virtual ICollection<RawMaterialPurchase> RawMaterialPurchases { get; set; } = new List<RawMaterialPurchase>();
}
