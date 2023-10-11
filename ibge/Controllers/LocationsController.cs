using ibge.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ibge.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class LocationsController : ControllerBase
{
	private readonly LocationContext _context;

	public LocationsController(LocationContext context)
	{
		_context = context;
	}

	[HttpGet(Name = "GetListOfIbge")]
	public async Task<ActionResult<List<Location>>> Get()
	{
		var locations = await _context.Locations.ToListAsync();

		return Ok(locations);
	}
}
