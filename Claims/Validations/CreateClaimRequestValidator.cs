using Claims.Contracts.Requests;
using Claims.Domain.Aggregates;
using FluentValidation;

namespace Claims.Validations;

public class CreateClaimRequestValidator : AbstractValidator<CreateClaimRequest>
{
    public CreateClaimRequestValidator()
    {
        RuleFor(x => x.CoverId)
            .NotEmpty()
            .WithMessage("CoverId is mandatory");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is mandatory");
        
        RuleFor(x => x.DamageCost)
            .GreaterThan(0)
            .LessThanOrEqualTo(Claim.MaxAllowedDamageCost)
            .WithMessage($"Damage cost cannot exceed {Claim.MaxAllowedDamageCost}");
    }
}