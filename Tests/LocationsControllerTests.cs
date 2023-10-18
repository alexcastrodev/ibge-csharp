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
    private const int id = 1100015;

    [TestInitialize]
    public void Initialize()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(_options);
        _service = new LocationService(_context);

        _controller = new LocationsController(_service);
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
            Id = id,
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
            Id = id,
            City = "Alta Floresta D'Oeste",
            State = "RO",
        };

        // Act
        await _controller.Create(location);
        var result = await _controller.Get(searchCriteria);

        // Assert
        Assert.AreEqual(0, result.Value?.Count);
    }

    [TestMethod]
    public async Task GetById_Locations_ReturnsLocations()
    {
        // arrange
        Location location = new()
        {
            Id = id,
            City = "Alta Floresta D'Oeste",
            State = "RO",
        };

        // Act
        await _controller.Create(location);
        var result = await _controller.Index(id);

        // Assert
        Assert.AreEqual(id, result.Value?.Id);
    }

    [TestMethod]

    public async Task deleteLocation_Locations_ReturnsLocations()
    {
        // arrange
        Location location = new()
        {
            Id = id,
            City = "Alta Floresta D'Oeste",
            State = "RO",
        };

        // Act
        await _controller.Create(location);
        await _controller.Delete(id);
        var result = await _controller.Index(id);

        // Assert
        Assert.AreEqual(null, result.Value);
    }

    [TestMethod]
    public async Task updateLocation_Locations_ReturnsLocations()
    {
        // arrange
        Location location = new()
        {
            Id = id,
            City = "Alta Floresta D'Oeste",
            State = "RO",
        };

        LocationUpdate locationUpdated = new()
        {
            City = "Rio de Janeiro",
            State = "RJ",
        };

        // Act
        await _controller.Create(location);
        var result = await _controller.Update(id, locationUpdated);

        // Assert
        Assert.AreEqual("RJ", result.Value?.State);
        Assert.AreEqual("Rio de Janeiro", result.Value?.City);
    }

    [TestMethod]
    public async Task patchLocation_Locations_ReturnsLocations()
    {
        // arrange
        Location location = new()
        {
            Id = id,
            City = "Alta Floresta D'Oeste",
            State = "RO",
        };

        LocationPatch locationUpdated = new()
        {
            City = "Rio de Janeiro",
        };

        // Act
        await _controller.Create(location);
        var updatedData = await _controller.Patch(id, locationUpdated);
        List<string?> result = new() { updatedData.Value?.State.ToString(), updatedData.Value?.City.ToString() };

        // Assert
        Assert.AreEqual("RO", updatedData.Value?.State);
        Assert.AreEqual("Rio de Janeiro", updatedData.Value?.City);
    }


    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }
}