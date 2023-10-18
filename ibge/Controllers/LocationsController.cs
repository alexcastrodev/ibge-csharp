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
	[HttpGet(Name = "Get locations")]
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
	
	[Authorize]
	[HttpGet("{id}", Name = "Get location by id")]
	public async Task<ActionResult<Location>> Index([FromRoute] int id)
	{
		var location = await _locationService.Find(id);
		if (location.Value == null) return NotFound();

		return location.Value;
	}

	[Authorize] [HttpDelete("{id}", Name = "Delete location by id")]
	public async Task<ActionResult<List<bool>>> Delete([FromRoute] int id)
	{
		var location = await _locationService.Delete(id);
		if (location.Value == 0) return NotFound();		
		return Ok(location.Value);
	}
	
	[Authorize] [HttpPut("{id}", Name = "Update location by id")]
	public async Task<ActionResult<Location>> Update([FromRoute] int id, [FromBody] LocationUpdate model)
	{
		var location = await _locationService.Update(id, model);
		if (location.Value == null) return NotFound();		
		return location.Value;
	}
	
	[Authorize] [HttpPatch("{id}", Name = "Patch location by id")]
	public async Task<ActionResult<Location>> Patch([FromRoute] int id, [FromBody] LocationPatch model)
	{
		var location = await _locationService.Patch(id, model);
		if (location.Value == null) return NotFound();
		return location.Value;
	}
}