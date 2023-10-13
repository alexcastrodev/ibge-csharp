using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ibge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ibge.ViewModels;

namespace ibge.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UsersController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost(Name = "Store User")]
    public async Task<ActionResult<bool>> CreateUserAsync(
        [FromBody] User model
        )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        model.Password = HashPassword(model.Password);
        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
            {
                return Conflict("Email already exists");
            }

            throw new Exception(ex.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<ActionResult<bool>> Login(
        [FromBody] User model
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await _context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found");
        }

        if (user.Password != HashPassword(model.Password))
        {
            return Unauthorized("Invalid password");
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.Name, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        UserViewModel userData = new()
        {
            Id = user.Id,
            Email = user.Email
        };

        return Ok(new
        {
            user = userData,
            token = tokenString
        });
    }

    private string HashPassword(string password)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
    }


}
