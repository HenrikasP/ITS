using Claims.Domain.Aggregates.Enums;

namespace Claims.Domain.Aggregates;

public class Cover
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public CoverType Type { get; set; }
    public decimal Premium { get; set; }
    
    public static Cover Create(DateOnly startDate, DateOnly endDate, CoverType type, decimal premium)
    {
        return new Cover
        {
            Id = Guid.NewGuid(),
            StartDate = startDate,
            EndDate = endDate,
            Type = type,
            Premium = premium
        };
    }
}