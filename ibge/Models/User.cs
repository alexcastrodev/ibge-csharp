using System.ComponentModel.DataAnnotations;

namespace ibge.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		[EmailAddress(ErrorMessage = "Invalid email address")]
		[Required(ErrorMessage = "Email is required")]
		public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }

}

