using Claims.Domain.Aggregates.Enums;

namespace Claims.Domain.Aggregates;

public class Claim
{
    public Guid Id { get; set; }
    public Guid CoverId { get; set; }
    public DateOnly Created { get; set; }
    public string Name { get; set; } = null!;
    public ClaimType Type { get; set; }
    public decimal DamageCost { get; set; }

    public static decimal MaxAllowedDamageCost = 100000;

    public static Claim Create(Guid coverId, string name, ClaimType type, decimal damageCost, DateOnly created)
    {
        if (damageCost > MaxAllowedDamageCost)
            throw new InvalidDataException($"{nameof(DamageCost)} cannot exceed {MaxAllowedDamageCost}");

        if (damageCost <= 0)
            throw new InvalidDataException($"{nameof(DamageCost)} cannot be less than 0");
        
        return new Claim
        {
            Id = Guid.NewGuid(),
            CoverId = coverId,
            Name = name,
            Type = type,
            DamageCost = damageCost,
            Created = created
        };
    }
}