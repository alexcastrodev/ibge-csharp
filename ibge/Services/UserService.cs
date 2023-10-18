using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ibge.Dtos;
using ibge.Exceptions;
using ibge.Models;
using ibge.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ibge.Services;

public class UserService : IUserRepository
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<bool>> Create(UserParams model)
    {
        var user = await _context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();

        if (user != null) throw ConflictException.Create("Email already exists");

        user = new User
        {
            Email = model.Email,
            Password = HashPassword(model.Password)
        };

        _context.Add(user);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ActionResult<LoggedUser>> Login(UserParams model, string jwtKey)
    {
        var user = await _context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();

        if (user == null) throw NotFoundException.Create("User not found");

        if (user.Password != HashPassword(model.Password)) throw new Exception("Invalid password");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        LoggedUser loggedUser = new()
        {
            Id = user.Id,
            Email = user.Email,
            Token = tokenString
        };

        return loggedUser;
    }

    private string HashPassword(string password)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
    }
}