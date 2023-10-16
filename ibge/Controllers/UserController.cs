using ibge.Dtos;
using ibge.Exceptions;
using ibge.Repositories;
using Microsoft.AspNetCore.Mvc;

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
			if (ex is ConflictException) return Conflict(ex.Message);
			throw new Exception(ex.Message);
		}
	}

	[HttpPost("Login")]
	public async Task<ActionResult<LoggedUser>> Login(
		[FromBody] UserParams model
	)
	{
		try
		{
			var jwtKey = _configuration["JwtSettings:Key"] ?? "";
			var user = await _userService.Login(model, jwtKey);

			return Ok(user.Value);
		}
		catch (Exception ex)
		{
			if (ex is NotFoundException) return NotFound(ex.Message);

			throw new Exception(ex.Message);
		}
	}
}