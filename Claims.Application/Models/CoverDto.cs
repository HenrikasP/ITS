using Claims.Application.Models.Enums;

namespace Claims.Application.Models;

public class CoverDto
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public CoverType Type { get; set; }
    public decimal Premium { get; set; }
}