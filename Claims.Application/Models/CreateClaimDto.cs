using System.ComponentModel.DataAnnotations;
using Claims.Application.Models.Enums;

namespace Claims.Application.Models;

public class CreateClaimDto
{
    public Guid CoverId { get; set; }
    public DateOnly Created { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public ClaimType Type { get; set; }
    public decimal DamageCost { get; set; }
}