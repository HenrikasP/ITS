using Claims.Application.Models;
using Claims.Application.Services.Claims;
using Claims.Domain.Aggregates;
using Claims.Events;
using Claims.Events.Enums;
using Claims.Infrastructure.Persistence.Entities;
using Claims.Infrastructure.Persistence.Repositories;
using Claims.Tests.Common;
using MapsterMapper;
using MassTransit;
using Moq;
using Xunit;

namespace Claims.Application.Tests.Services.Claims;

public class ClaimsServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IClaimsRepository> _claimsRepositoryMock;
    private readonly Mock<ICoversRepository> _coversRepositoryMock;
    private readonly Mock<IBus> _busMock;

    private readonly ClaimsService _service;

    public ClaimsServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _claimsRepositoryMock = new Mock<IClaimsRepository>();
        _coversRepositoryMock = new Mock<ICoversRepository>();
        _busMock = new Mock<IBus>();

        _service = new ClaimsService(_mapperMock.Object, _claimsRepositoryMock.Object, _coversRepositoryMock.Object, _busMock.Object);
    }
    
    [Theory]
    [CustomAutoData]
    public async Task CreateAsync_ShouldFail_IfCoverNotFound(CreateClaimDto request)
    {
        // Arrange
        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _claimsRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<ClaimEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _busMock.Verify(m => m.Publish(It.IsAny<ClaimAuditEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.True(result.IsFailed);
    }
    
    [Theory]
    [InlineAutoMoqData(true)]
    [InlineAutoMoqData(false)]
    public async Task CreateAsync_ShouldFail_IfClaimCreatedIsNotInCoverRange(
        bool claimIsEarlierThanCover,
        CreateClaimDto request,
        CoverEntity coverEntity,
        DateTime startDate,
        int coverActivityNumberOfDays
        )
    {
        // Arrange
        coverEntity.StartDate = startDate;
        coverEntity.EndDate = startDate.AddDays(coverActivityNumberOfDays);
            
        request.Created = claimIsEarlierThanCover ?
            DateOnly.FromDateTime(coverEntity.StartDate).AddDays(-1) :
            DateOnly.FromDateTime(coverEntity.EndDate).AddDays(1);

        _coversRepositoryMock.Setup(m => m.GetAsync(request.CoverId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(coverEntity);
        
        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _claimsRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<ClaimEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _busMock.Verify(m => m.Publish(It.IsAny<ClaimAuditEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.True(result.IsFailed);
    }
    
    [Theory]
    [CustomAutoData]
    public async Task CreateAsync_ShouldCreateAndPublishMessage(
        CreateClaimDto request,
        ClaimEntity claimEntity,
        ClaimDto claimDto,
        CoverEntity coverEntity)
    {
        // Arrange
        request.Created = DateOnly.FromDateTime(coverEntity.StartDate);
        coverEntity.EndDate = coverEntity.StartDate.AddDays(10);
        _coversRepositoryMock.Setup(m => m.GetAsync(request.CoverId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(coverEntity);

        _claimsRepositoryMock.Setup(m => m.CreateAsync(claimEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(claimEntity);

        _mapperMock.Setup(m => m.Map<ClaimEntity>(It.IsAny<Claim>()))
            .Returns(claimEntity);

        _mapperMock.Setup(m => m.Map<ClaimDto>(claimEntity))
            .Returns(claimDto);
        
        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _claimsRepositoryMock.Verify(m => m.CreateAsync(claimEntity, It.IsAny<CancellationToken>()), Times.Once);
        _busMock.Verify(
            m => m.Publish(
                It.Is((ClaimAuditEvent @event) =>
                    @event.ClaimId == claimEntity.Id && @event.HttpRequestType == HttpRequestType.POST),
                It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(claimDto, result.Value);
    }

    [Theory]
    [CustomAutoData]
    public async Task GetAsync_ShouldReturnFail_IfNotFoundInRepo(Guid id)
    {
        // Arrange
        // Act
        var result = await _service.GetAsync(id, CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map<ClaimDto>(It.IsAny<ClaimEntity>()), Times.Never);
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [CustomAutoData]
    public async Task GetAsync_ShouldReturnItem_IfFoundInRepo(
        Guid id,
        ClaimEntity entity,
        ClaimDto dto)
    {
        // Arrange
        _claimsRepositoryMock.Setup(m => m.GetAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        _mapperMock.Setup(m => m.Map<ClaimDto>(entity))
            .Returns(dto);

        // Act
        var result = await _service.GetAsync(id, CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map<ClaimDto>(It.IsAny<ClaimEntity>()), Times.Once);
        Assert.True(result.IsSuccess);
        Assert.Equal(dto, result.Value);
    }

    [Theory]
    [CustomAutoData]
    public async Task GetListAsync_ShouldReturnItemsFoundInRepo(
        IEnumerable<ClaimEntity> entities,
        IEnumerable<ClaimDto> dtos)
    {
        // Arrange
        _claimsRepositoryMock.Setup(m => m.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);

        _mapperMock.Setup(m => m.Map<IEnumerable<ClaimDto>>(entities))
            .Returns(dtos);

        // Act
        var result = await _service.GetListAsync(CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map<IEnumerable<ClaimDto>>(It.IsAny<IEnumerable<ClaimEntity>>()), Times.Once);
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
        _claimsRepositoryMock.Verify(m => m.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _busMock.Verify(
            m => m.Publish(
                It.Is((ClaimAuditEvent @event) =>
                    @event.ClaimId == id && @event.HttpRequestType == HttpRequestType.DELETE),
                It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.IsSuccess);
    }
}
