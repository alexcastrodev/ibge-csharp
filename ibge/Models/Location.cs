using System.ComponentModel.DataAnnotations;

namespace ibge.Models;

public class Location
{
	[Key] public required string Id { get; set; }

	[MaxLength(100)]
	[Required(ErrorMessage = "State is required")]
	public required string State { get; set; }

	[MaxLength(100)]
	[Required(ErrorMessage = "City is required")]
	public required string City { get; set; }
}