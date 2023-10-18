using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ibge.Models;

[Index(nameof(Id), nameof(State), nameof(City))]
public class Location
{
    [Required(ErrorMessage = "Id is required")]
    public required int Id { get; set; }

    [MaxLength(2)]
    [Required(ErrorMessage = "State is required")]
    public required string State { get; set; }

    [MaxLength(100)]
    [Required(ErrorMessage = "City is required")]
    public required string City { get; set; }
}