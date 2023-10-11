using ibge.Controllers;
using ibge.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests;
[TestClass]
public class LocationTest
{
	[TestMethod]
	public async Task Get_Locations_ReturnsEmpty()
	{
        // Arrange
        DbContextOptions<LocationContext> options = new DbContextOptionsBuilder<LocationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        LocationContext context = new(options);
        LocationsController controller = new(context);

        // Act
        ActionResult<List<Location>> result = await controller.Get();

        // Assert
        Assert.AreEqual(0, result.Value?.Count);
    }
}
