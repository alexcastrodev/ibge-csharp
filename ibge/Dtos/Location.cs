using System.ComponentModel.DataAnnotations;

namespace ibge.Dtos;

public class LocationSearchCriteria
{	
	public int? Id { get; set; }
	public string? City { get; set; }
	public string? State { get; set; }
}

public class LocationUpdate
{	
	[Required(ErrorMessage = "City is required")]
	public required string City { get; set; }

	[Required(ErrorMessage = "State is required")]
	public required string State { get; set; }
}

public class LocationPatch
{	
	public string? City { get; set; }
	public string? State { get; set; }
}