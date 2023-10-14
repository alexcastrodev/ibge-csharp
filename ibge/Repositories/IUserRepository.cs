using ibge.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ibge.Repository;

public interface IUserRepository
{
	Task<ActionResult<bool>> Create(UserParams model);
	Task<ActionResult<LoggedUser>> Login(UserParams model, string jwtKey);
}