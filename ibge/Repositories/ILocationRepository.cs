using ibge.Dtos;
using ibge.Models;
using Microsoft.AspNetCore.Mvc;

namespace ibge.Repositories;

public interface ILocationRepository
{
    Task<ActionResult<bool>> Create(Location model);
    Task<ActionResult<List<Location>>> Get(LocationSearchCriteria searchCriteria);
    Task<ActionResult<Location?>> Find(int id);
    Task<ActionResult<int>> Delete(int id);
    Task<ActionResult<Location?>> Update(int id, LocationUpdate model);
    Task<ActionResult<Location?>> Patch(int id, LocationPatch model);
    List<int> GetIds();
}