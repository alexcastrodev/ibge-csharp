using Microsoft.EntityFrameworkCore;
using ibge.Entities;

public class LocationContext : DbContext
{
	public LocationContext(DbContextOptions options) : base(options) { }
	public DbSet<Location> Locations { get; set; }
}

