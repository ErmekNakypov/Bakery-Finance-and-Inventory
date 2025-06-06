using System.ComponentModel.DataAnnotations;

namespace Production.DTO;

public class UpdateEmployeeDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Fullname is required.")]
    public string FullName { get; set; } = null!;
    
    [Required(ErrorMessage = "Address is required.")]
    public string? Address { get; set; }
    
    [Required(ErrorMessage = "Phone is required.")]
    public string? Phone { get; set; }
}