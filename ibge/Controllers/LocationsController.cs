using ibge.Dtos;
using ibge.Exceptions;
using ibge.Models;
using ibge.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ibge.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class LocationsController : ControllerBase
{
	private readonly ILocationRepository _locationService;

	public LocationsController(ILocationRepository locationService)
	{
		_locationService = locationService;
	}

	[Authorize]
	[HttpGet(Name = "GetListOfIbge")]
	public async Task<ActionResult<List<Location>>> Get([FromQuery] LocationSearchCriteria searchCriteria)
	{
		return await _locationService.Get(searchCriteria);
	}

	[Authorize]
	[HttpPost(Name = "Create a location")]
	public async Task<ActionResult<bool>> Create(
		[FromBody] Location model
	)
	{
		try
		{
			var createLocation = await _locationService.Create(model);
			return createLocation.Value;
		}
		catch (Exception ex)
		{
			if (ex is ConflictException) return Conflict(ex.Message);
			throw new Exception(ex.Message);
		}
	}
}