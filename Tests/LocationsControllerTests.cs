using ibge.Controllers;
using ibge.Dtos;
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


	[TestCleanup]
	public void Cleanup()
	{
		_context.Dispose();
	}
}