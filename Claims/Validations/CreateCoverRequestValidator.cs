using Claims.Contracts.Requests;
using FluentValidation;

namespace Claims.Validations;

public class CreateCoverRequestValidator : AbstractValidator<CreateCoverRequest>
{
    public CreateCoverRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("StartDate cannot be in the past");
        
        RuleFor(x => x.EndDate)
            .Must((model, endDate) => endDate <= model.StartDate.AddYears(1))
            .WithMessage("Total insurance period cannot exceed 1 year");
    }
}