using Claims.Application.Models.Enums;
using FluentResults;

namespace Claims.Application.Services.PremiumCalculation;

public interface IPremiumCalculationService
{
    Result<decimal> ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType);
}