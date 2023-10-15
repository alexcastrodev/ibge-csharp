using ibge.Controllers;
using ibge.Models;
using ibge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        // Act
        ActionResult<List<Location>> result = await _controller.Get();

        // Assert
        Assert.AreEqual(0, result.Value?.Count);
    }
}
