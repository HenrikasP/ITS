using Claims.Application.Models.Enums;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Claims.Application.Services.PremiumCalculation;

public class PremiumCalculationService : IPremiumCalculationService
{
    private readonly ILogger<PremiumCalculationService> _logger;

    public PremiumCalculationService(ILogger<PremiumCalculationService> logger)
    {
        _logger = logger;
    }

    public Result<decimal> ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        var insuranceLength = endDate.DayNumber - startDate.DayNumber;
        var totalPremium = 0m;
        
        var normalPriceNumberOfDays = Math.Min(30, insuranceLength);
        var discountedPriceNumberOfDays = insuranceLength > 30 ? Math.Min(180, insuranceLength) - 30 : 0;
        var reducedPriceNumberOfDays = insuranceLength > 180 ? insuranceLength - 180 : 0;
        
        var baseDayRate = 1250;
        var normalMultiplier = GetMultiplierByCoverTypeAndPriceType(coverType, PriceType.Normal);
        var reducedMultiplier = GetMultiplierByCoverTypeAndPriceType(coverType, PriceType.Reduced);
        var discountedMultiplier = GetMultiplierByCoverTypeAndPriceType(coverType, PriceType.Discounted);
        if (normalMultiplier.IsFailed || reducedMultiplier.IsFailed || discountedMultiplier.IsFailed)
        {
            var errors = normalMultiplier.Errors
                .Concat(reducedMultiplier.Errors)
                .Concat(discountedMultiplier.Errors);

            _logger.LogError("Failed to calculate premium for: {StartDate} {EndDate} {CoverType}",
                startDate, endDate, coverType);
            return Result.Fail(errors);
        }
        
        totalPremium += normalPriceNumberOfDays * baseDayRate * normalMultiplier.Value;
        totalPremium += discountedPriceNumberOfDays * baseDayRate * reducedMultiplier.Value;
        totalPremium += reducedPriceNumberOfDays * baseDayRate * discountedMultiplier.Value;
        
        return Result.Ok(totalPremium);
    }

    private static Result<decimal> GetMultiplierByCoverTypeAndPriceType(CoverType coverType, PriceType priceType)
    {
        var otherTypesReduceMultiplier = .02m;
        var otherTypesDiscountMultiplier = .03m;
        var multiplier = (coverType, priceType) switch
        {
            (CoverType.Yacht, PriceType.Normal)  => 1.1m,
            (CoverType.Yacht, PriceType.Reduced)  => 1.05m,
            (CoverType.Yacht, PriceType.Discounted)  => 1.02m,
            (CoverType.PassengerShip, PriceType.Normal) => 1.2m,
            (CoverType.PassengerShip, PriceType.Reduced) => 1.2m - otherTypesReduceMultiplier,
            (CoverType.PassengerShip, PriceType.Discounted) => 1.2m - otherTypesDiscountMultiplier,
            (CoverType.Tanker, PriceType.Normal) => 1.5m,
            (CoverType.Tanker, PriceType.Reduced) => 1.5m - otherTypesReduceMultiplier,
            (CoverType.Tanker, PriceType.Discounted) => 1.5m - otherTypesDiscountMultiplier,
            (_, PriceType.Normal) => 1.3m,
            (_, PriceType.Reduced) => 1.3m - otherTypesReduceMultiplier,
            (_, PriceType.Discounted) => 1.3m - otherTypesDiscountMultiplier,
            _ => Result.Fail<decimal>($"Passed coverType {coverType} and priceType {priceType} is not supported")
        };

        return multiplier;
    }
}