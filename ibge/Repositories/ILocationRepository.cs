using ibge.Models;
using Microsoft.AspNetCore.Mvc;

namespace ibge.Repositories;

public interface ILocationRepository
{
	Task<ActionResult<bool>> Create(Location model);
	Task<ActionResult<List<Location>>> Get();
}