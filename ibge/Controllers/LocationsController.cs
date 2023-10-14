using ibge.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ibge.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class LocationsController : ControllerBase
{
	private readonly AppDbContext _context;

	public LocationsController(AppDbContext context)
	{
		_context = context;
	}

	[Authorize]
	[HttpGet(Name = "GetListOfIbge")]
	public async Task<ActionResult<List<Location>>> Get()
	{
		var locations = await _context.Locations.ToListAsync();

		return locations;
	}
}