using ibge.Models;
using Microsoft.AspNetCore.Mvc;

namespace ibge.Repository;

public interface ILocationRepository
{
	Task<ActionResult<bool>> Create(Location model);
	Task<ActionResult<List<Location>>> Get();
}