using Claims.Application.Models.Enums;

namespace Claims.Application.Models;

public class ClaimDto
{
    public Guid Id { get; set; }
    public Guid CoverId { get; set; }
    public DateOnly Created { get; set; }
    public string Name { get; set; } = null!;
    public ClaimType Type { get; set; }
    public decimal DamageCost { get; set; }
}