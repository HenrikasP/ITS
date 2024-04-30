using Claims.Application.Models;
using Claims.Application.Services.Covers;
using Claims.Application.Services.PremiumCalculation;
using Claims.Domain.Aggregates;
using Claims.Events;
using Claims.Events.Enums;
using Claims.Infrastructure.Persistence.Entities;
using Claims.Infrastructure.Persistence.Repositories;
using Claims.Tests.Common;
using FluentResults;
using MapsterMapper;
using MassTransit;
using Moq;
using Xunit;

namespace Claims.Application.Tests.Services.Covers;

public class CoversServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICoversRepository> _repositoryMock;
    private readonly Mock<IBus> _busMock;
    private readonly Mock<IPremiumCalculationService> _premiumCalculationService;

    private readonly CoversService _service;

    public CoversServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _repositoryMock = new Mock<ICoversRepository>();
        _busMock = new Mock<IBus>();
        _premiumCalculationService = new Mock<IPremiumCalculationService>();

        _service = new CoversService(_mapperMock.Object, _busMock.Object, _repositoryMock.Object, _premiumCalculationService.Object);
    }
    
    [Theory]
    [InlineAutoMoqData(1)]
    [InlineAutoMoqData(10)]
    [InlineAutoMoqData(100)]
    public async Task CreateAsync_ShouldFail_IfCoverCreatedIsInThePast(int numberOfDaysInPast, CreateCoverDto request)
    {
        // Arrange
        request.StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(- numberOfDaysInPast);
        
        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(m => m.CreateAsync(It.IsAny<CoverEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _busMock.Verify(m => m.Publish(It.IsAny<CoverAuditEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.True(result.IsFailed);
    }
    
    [Theory]
    [CustomAutoData]
    public async Task CreateAsync_ShouldFail_IfCoverPeriodIsLongerThanYear(
        CreateCoverDto request)
    {
        // Arrange
        request.StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
        request.EndDate = request.StartDate.AddYears(1).AddDays(1);
        
        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(m => m.CreateAsync(It.IsAny<CoverEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _busMock.Verify(m => m.Publish(It.IsAny<CoverAuditEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.True(result.IsFailed);
    }
    
    [Theory]
    [CustomAutoData]
    public async Task CreateAsync_ShouldFail_IfPremiumCalculationFailed(
        CreateCoverDto request)
    {
        // Arrange
        request.StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
        request.EndDate = request.StartDate.AddDays(10);
        
        _premiumCalculationService.Setup(m => m.ComputePremium(request.StartDate, request.EndDate, request.Type))
            .Returns(Result.Fail(string.Empty));
        
        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(m => m.CreateAsync(It.IsAny<CoverEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _busMock.Verify(m => m.Publish(It.IsAny<CoverAuditEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.True(result.IsFailed);
    }
    
    [Theory]
    [CustomAutoData]
    public async Task CreateAsync_ShouldCreateAndPublishMessage(
        CreateCoverDto request,
        decimal premium,
        CoverEntity coverEntity,
        CoverDto coverDto)
    {
        // Arrange
        request.StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
        request.EndDate = request.StartDate.AddDays(10);

        _premiumCalculationService.Setup(m =>
                m.ComputePremium(request.StartDate, request.EndDate, request.Type))
            .Returns(Result.Ok(premium));

        _repositoryMock.Setup(m => m.CreateAsync(coverEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(coverEntity);

        _mapperMock.Setup(m => m.Map<CoverEntity>(It.IsAny<Cover>()))
            .Returns(coverEntity);

        _mapperMock.Setup(m => m.Map<CoverDto>(coverEntity))
            .Returns(coverDto);
        
        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(m => m.CreateAsync(coverEntity, It.IsAny<CancellationToken>()), Times.Once);
        _busMock.Verify(
            m => m.Publish(
                It.Is((CoverAuditEvent @event) =>
                    @event.CoverId == coverEntity.Id && @event.HttpRequestType == HttpRequestType.POST),
                It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(coverDto, result.Value);
    }

    [Theory]
    [CustomAutoData]
    public async Task GetAsync_ShouldReturnFail_IfNotFoundInRepo(Guid id)
    {
        // Arrange
        // Act
        var result = await _service.GetAsync(id, CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map<CoverDto>(It.IsAny<CoverEntity>()), Times.Never);
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [CustomAutoData]
    public async Task GetAsync_ShouldReturnItem_IfFoundInRepo(
        Guid id,
        CoverEntity entity,
        CoverDto dto)
    {
        // Arrange
        _repositoryMock.Setup(m => m.GetAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        _mapperMock.Setup(m => m.Map<CoverDto>(entity))
            .Returns(dto);

        // Act
        var result = await _service.GetAsync(id, CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map<CoverDto>(It.IsAny<CoverEntity>()), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(dto, result.Value);
    }

    [Theory]
    [CustomAutoData]
    public async Task GetListAsync_ShouldReturnItemsFoundInRepo(
        IEnumerable<CoverEntity> entities,
        IEnumerable<CoverDto> dtos)
    {
        // Arrange
        _repositoryMock.Setup(m => m.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);

        _mapperMock.Setup(m => m.Map<IEnumerable<CoverDto>>(entities))
            .Returns(dtos);

        // Act
        var result = await _service.GetListAsync(CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map<IEnumerable<CoverDto>>(It.IsAny<IEnumerable<CoverEntity>>()), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(dtos, result.Value);
    }

    [Theory]
    [CustomAutoData]
    public async Task DeleteAsync_ShouldDeleteAndPublishMessage(Guid id)
    {
        // Arrange
        // Act
        var result = await _service.DeleteAsync(id, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(m => m.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _busMock.Verify(
            m => m.Publish(
                It.Is((CoverAuditEvent @event) =>
                    @event.CoverId == id && @event.HttpRequestType == HttpRequestType.DELETE),
                It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.IsSuccess);
    }
}
