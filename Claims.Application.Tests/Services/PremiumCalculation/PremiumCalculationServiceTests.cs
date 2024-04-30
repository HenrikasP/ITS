using Claims.Application.Models.Enums;
using Claims.Application.Services.PremiumCalculation;
using Claims.Tests.Common;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Claims.Application.Tests.Services.PremiumCalculation;

public class PremiumCalculationServiceTests
{
    private readonly NullLogger<PremiumCalculationService> _logger;

    private readonly PremiumCalculationService _service;

    public PremiumCalculationServiceTests()
    {
        _logger = new NullLogger<PremiumCalculationService>();

        _service = new PremiumCalculationService(_logger);
    }
    
    [Theory]
    [InlineAutoMoqData(CoverType.Yacht, 1, 1375)]
    [InlineAutoMoqData(CoverType.Yacht, 10, 13750)]
    [InlineAutoMoqData(CoverType.Yacht, 30, 41250)]
    [InlineAutoMoqData(CoverType.Yacht, 150, 198750)]
    [InlineAutoMoqData(CoverType.Yacht, 180, 238125)]
    [InlineAutoMoqData(CoverType.Yacht, 210, 276375)]
    [InlineAutoMoqData(CoverType.PassengerShip, 1, 1500)]
    [InlineAutoMoqData(CoverType.PassengerShip, 10, 15000)]
    [InlineAutoMoqData(CoverType.PassengerShip, 30, 45000)]
    [InlineAutoMoqData(CoverType.PassengerShip, 150, 222000)]
    [InlineAutoMoqData(CoverType.PassengerShip, 180, 266250)]
    [InlineAutoMoqData(CoverType.PassengerShip, 210, 310125)]
    [InlineAutoMoqData(CoverType.ContainerShip, 1, 1625)]
    [InlineAutoMoqData(CoverType.ContainerShip, 10, 16250)]
    [InlineAutoMoqData(CoverType.ContainerShip, 30, 48750)]
    [InlineAutoMoqData(CoverType.ContainerShip, 150, 240750)]
    [InlineAutoMoqData(CoverType.ContainerShip, 180, 288750)]
    [InlineAutoMoqData(CoverType.ContainerShip, 210, 336375)]
    [InlineAutoMoqData(CoverType.BulkCarrier, 1, 1625)]
    [InlineAutoMoqData(CoverType.BulkCarrier, 10, 16250)]
    [InlineAutoMoqData(CoverType.BulkCarrier, 30, 48750)]
    [InlineAutoMoqData(CoverType.BulkCarrier, 150, 240750)]
    [InlineAutoMoqData(CoverType.BulkCarrier, 180, 288750)]
    [InlineAutoMoqData(CoverType.BulkCarrier, 210, 336375)]
    [InlineAutoMoqData(CoverType.Tanker, 1, 1875)]
    [InlineAutoMoqData(CoverType.Tanker, 10, 18750)]
    [InlineAutoMoqData(CoverType.Tanker, 30, 56250)]
    [InlineAutoMoqData(CoverType.Tanker, 150, 278250)]
    [InlineAutoMoqData(CoverType.Tanker, 180, 333750)]
    [InlineAutoMoqData(CoverType.Tanker, 210, 388875)]
    public void CreateAsync_ShouldFail_IfCoverCreatedIsInThePast(CoverType type, int numberOfDays, decimal expected, DateOnly startDate)
    {
        // Arrange
        var endDate = startDate.AddDays(numberOfDays);
        
        // Act
        var result = _service.ComputePremium(startDate, endDate, type);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value);
    }
}