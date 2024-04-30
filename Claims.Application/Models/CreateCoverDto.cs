using Claims.Application.Models.Enums;

namespace Claims.Application.Models;

public class CreateCoverDto
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public CoverType Type { get; set; }
}