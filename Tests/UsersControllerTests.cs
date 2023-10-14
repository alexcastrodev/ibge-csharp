using ibge.Controllers;
using ibge.Dto;
using ibge.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests;

[TestClass]
public class UsersTest
{
	private DbContextOptions<AppDbContext> _options;
	private AppDbContext _context;
	private UserService _userService;
	private IConfiguration _configuration;
	private UsersController _controller;

	[TestInitialize]
	public void Initialize()
	{
		_options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		_context = new AppDbContext(_options);
		_userService = new UserService(_context);

		_configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();

		_controller = new UsersController(_userService, _configuration);
	}


	[TestMethod]
	public async Task Post_User_ReturnsTruthy()
	{
		// Arrange
		UserParams model = new()
		{
			Email = "teste@teste.com",
			Password = "12345678"
		};

		// Act
		var result = await _controller.CreateUserAsync(model);

		// Assert
		Assert.AreEqual(true, result.Value);
	}

	[TestCleanup]
	public void Cleanup()
	{
		_context.Dispose();
	}
}