using System.ComponentModel.DataAnnotations;

namespace ibge.Models;

public class User
{
	[Key] public int Id { get; set; }

	[MaxLength(200)] [Required] public string Email { get; set; }

	[MaxLength(64)] [Required] public string Password { get; set; }
}