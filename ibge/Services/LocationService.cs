using ibge.Exceptions;
using ibge.Models;
using ibge.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ibge.Services;

public class LocationService : ILocationRepository
{
	private readonly AppDbContext _context;

	public LocationService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<ActionResult<List<Location>>> Get()
	{
		var locations = await _context.Locations.ToListAsync();
		return locations;
	}
	
	public async Task<ActionResult<bool>> Create(Location model)
	{
		var location = _context.Locations.Count(location => location.Id == model.Id);

		if (location > 0)
		{
			throw ConflictException.Create("Location already exists");
		}
		
		_context.Locations.Add(model);
		await _context.SaveChangesAsync();
		return true;
	}

}