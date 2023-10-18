using ibge.Dtos;
using ibge.Exceptions;
using ibge.Models;
using ibge.Repositories;
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

    public async Task<ActionResult<List<Location>>> Get(LocationSearchCriteria searchCriteria)
    {
        var query = _context.Locations.AsQueryable();

        if (searchCriteria.Id.HasValue)
        {
            query = query.Where(loc => loc.Id == searchCriteria.Id);
        }
        if (!string.IsNullOrEmpty(searchCriteria.City))
        {
            query = query.Where(loc => loc.City.Contains(searchCriteria.City));
        }

        if (!string.IsNullOrEmpty(searchCriteria.State))
        {
            query = query.Where(loc => loc.State.Contains(searchCriteria.State));
        }

        var locations = await query.ToListAsync();
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

    public async Task<ActionResult<Location?>> Find(int id)
    {
        var location = await _context.Locations.FindAsync(id);

        return location;
    }

    public async Task<ActionResult<int>> Delete(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null) return 0;

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();

        return location.Id;
    }

    public async Task<ActionResult<Location?>> Update(int id, LocationUpdate model)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null) return null!;

        location.City = model.City;
        location.State = model.State;

        await _context.SaveChangesAsync();

        return location;
    }

    public async Task<ActionResult<Location?>> Patch(int id, LocationPatch model)
    {
        var location = await _context.Locations.Where(location => location.Id == id).FirstOrDefaultAsync();
        if (location == null) return null!;

        if (!string.IsNullOrEmpty(model.City))
        {
            location.City = model.City;
        }

        if (!string.IsNullOrEmpty(model.State))
        {
            location.State = model.State;
        }

        await _context.SaveChangesAsync();

        return location;
    }
}