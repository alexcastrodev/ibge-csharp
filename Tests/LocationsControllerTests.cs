using ibge.Controllers;
using ibge.Models;
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
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        AppDbContext context = new(options);
        LocationsController controller = new(context);

        // Act
        ActionResult<List<Location>> result = await controller.Get();

        // Assert
        Assert.AreEqual(0, result.Value?.Count);
    }
}
