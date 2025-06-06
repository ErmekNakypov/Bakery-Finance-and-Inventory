using System.ComponentModel.DataAnnotations;

namespace Production.DTO;

public class LoginDto
{
    public int Id { get; set; }
    [Required]
    public string Login { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}