using ibge.Controllers;
using ibge.Dtos;
using ibge.Models;
using ibge.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests;

[TestClass]
public class LocationTest
{
	private DbContextOptions<AppDbContext> _options;
	private AppDbContext _context;
	private LocationService _service;
	private LocationsController _controller;

	[TestInitialize]
	public void Initialize()
	{
		_options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		_context = new AppDbContext(_options);
		_service = new LocationService(_context);

		_controller = new LocationsController(_context, _service);
	}

	[TestMethod]
	public async Task Get_Locations_ReturnsEmpty()
	{
		// arrange
		LocationSearchCriteria searchCriteria = new();
		
		// Act
		var result = await _controller.Get(searchCriteria);

		// Assert
		Assert.AreEqual(0, result.Value?.Count);
	}
	
	[TestMethod]
	public async Task Post_Locations_ReturnsLocations()
	{
		// arrange
		LocationSearchCriteria searchCriteria = new();
		Location location = new()
		{
			Id = 1100015,
			City = "Alta Floresta D'Oeste",
			State = "RO",
		};
		
		// Act
		await _controller.Create(location);
		var result = await _controller.Get(searchCriteria);

		// Assert
		Assert.AreEqual(1, result.Value?.Count);
	}
	
	[TestMethod]
	public async Task Get_LocationsWithCriteria_ReturnsEmpty()
	{
		// arrange
		LocationSearchCriteria searchCriteria = new()
		{
			Id = 1100010,
		};
		Location location = new()
		{
			Id = 1100015,
			City = "Alta Floresta D'Oeste",
			State = "RO",
		};
		
		// Act
		await _controller.Create(location);
		var result = await _controller.Get(searchCriteria);

		// Assert
		Assert.AreEqual(0, result.Value?.Count);
	}


	[TestCleanup]
	public void Cleanup()
	{
		_context.Dispose();
	}
}