using ibge.Dto;
using ibge.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ibge.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController : ControllerBase
{
	private readonly IUserRepository _userService;
	private readonly IConfiguration _configuration;

	public UsersController(IUserRepository userService, IConfiguration configuration)
	{
		_userService = userService;
		_configuration = configuration;
	}

	[HttpPost(Name = "Store User")]
	public async Task<ActionResult<bool>> CreateUserAsync(
		[FromBody] UserParams model
	)
	{
		try
		{
			var createUser = await _userService.Create(model);
			return createUser.Value;
		}
		catch (Exception ex)
		{
			if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
				return Conflict("Email already exists");

			throw new Exception(ex.Message);
		}
	}

	[HttpPost("Login")]
	public async Task<ActionResult<LoggedUser>> Login(
		[FromBody] UserParams model
	)
	{
		var jwtKey = _configuration["JwtSettings:Key"] ?? "";
		var user = await _userService.Login(model, jwtKey);

		return Ok(user.Value);
	}
}