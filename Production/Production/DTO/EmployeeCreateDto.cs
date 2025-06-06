using System.ComponentModel.DataAnnotations;

namespace Production.DTO;

public class EmployeeCreateDto
{
    [Required]
    public string Login { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Fullname is required.")]
    public string FullName { get; set; } = null!;
    
    [Required(ErrorMessage = "Salary is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0.")]
    public decimal Salary { get; set; }
    
    [Required(ErrorMessage = "Address is required.")]
    public string? Address { get; set; }
    
    [Required(ErrorMessage = "Phone is required.")]
    public string? Phone { get; set; }
    
}