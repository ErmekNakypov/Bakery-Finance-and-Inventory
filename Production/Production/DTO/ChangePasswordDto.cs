using System.ComponentModel.DataAnnotations;

namespace Production.DTO;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Current password is required")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string RepeatPassword { get; set; }
    
    
}