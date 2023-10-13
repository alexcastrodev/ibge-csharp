using ibge.Controllers;
using ibge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests;
[TestClass]
public class UsersTest
{
        [TestMethod]
        public async Task Post_User_ReturnsTruthy()
        {
                // Arrange
                DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;
                AppDbContext context = new(options);
                IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();
                UsersController controller = new(context, configuration);
                User model = new()
                {
                        Email = "teste@teste.com",
                        Password = "12345678",
                };

                // Act
                ActionResult<bool> result = await controller.CreateUserAsync(model);

                // Assert
                Assert.AreEqual(true, result.Value);
        }
}
