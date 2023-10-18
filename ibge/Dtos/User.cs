using System.ComponentModel.DataAnnotations;

namespace ibge.Dtos;

public class LoggedUser
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}

public class UserParams
{
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}