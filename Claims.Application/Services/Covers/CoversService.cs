using Claims.Application.Errors;
using Claims.Application.Models;
using Claims.Application.Services.PremiumCalculation;
using Claims.Domain.Aggregates;
using Claims.Events;
using Claims.Events.Enums;
using Claims.Infrastructure.Persistence.Entities;
using Claims.Infrastructure.Persistence.Repositories;
using FluentResults;
using MapsterMapper;
using MassTransit;
using CoverType = Claims.Domain.Aggregates.Enums.CoverType;

namespace Claims.Application.Services.Covers;

public class CoversService : ICoversService
{
    private readonly IMapper _mapper;
    private readonly IBus _bus;
    private readonly ICoversRepository _repository;
    private readonly IPremiumCalculationService _premiumCalculationService;

    public CoversService(
        IMapper mapper,
        IBus bus,
        ICoversRepository repository,
        IPremiumCalculationService premiumCalculationService)
    {
        _mapper = mapper;
        _bus = bus;
        _repository = repository;
        _premiumCalculationService = premiumCalculationService;
    }

    public async Task<Result<CoverDto>> CreateAsync(CreateCoverDto request, CancellationToken cancellationToken = default)
    {
        var validationResponse = Validate(request);
        if (validationResponse.IsFailed)
            return Result.Fail(validationResponse.Errors);
        
        var premiumResponse = _premiumCalculationService.ComputePremium(request.StartDate, request.EndDate, request.Type);
        if (premiumResponse.IsFailed)
            return Result.Fail(premiumResponse.Errors);

        var type = _mapper.Map<CoverType>(request.Type);
        var cover = Cover.Create(request.StartDate, request.EndDate, type, premiumResponse.Value);
        
        var entity = _mapper.Map<CoverEntity>(cover);
        var result = await _repository.CreateAsync(entity, cancellationToken);

        await _bus.Publish(new CoverAuditEvent(entity.Id, HttpRequestType.POST), cancellationToken);

        return Result.Ok(_mapper.Map<CoverDto>(result));
    }

    public async Task<Result<CoverDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(id, cancellationToken);
        if (result == default)
            return Result.Fail(new NotFoundError());

        return Result.Ok(_mapper.Map<CoverDto>(result));
    }

    public async Task<Result<IEnumerable<CoverDto>>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetListAsync(cancellationToken);

        return Result.Ok(_mapper.Map<IEnumerable<CoverDto>>(result));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        
        await _bus.Publish(new CoverAuditEvent(id, HttpRequestType.DELETE), cancellationToken);
        
        return Result.Ok();
    }

    private Result Validate(CreateCoverDto request)
    {
        if (request.StartDate < DateOnly.FromDateTime(DateTime.UtcNow))
            return Result.Fail(new BadRequestError());

        var maxEndDate = request.StartDate.AddYears(1);
        if (request.EndDate > maxEndDate)
            return Result.Fail(new BadRequestError());

        return Result.Ok();
    }
}