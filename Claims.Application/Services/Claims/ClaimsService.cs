using Claims.Application.Errors;
using Claims.Application.Models;
using Claims.Domain.Aggregates;
using Claims.Events;
using Claims.Events.Enums;
using Claims.Infrastructure.Persistence.Entities;
using Claims.Infrastructure.Persistence.Repositories;
using FluentResults;
using MapsterMapper;
using MassTransit;
using ClaimType = Claims.Domain.Aggregates.Enums.ClaimType;

namespace Claims.Application.Services.Claims;

public class ClaimsService : IClaimsService
{
    private readonly IMapper _mapper;
    private readonly IClaimsRepository _repository;
    private readonly ICoversRepository _coversRepository;
    private readonly IBus _bus;

    public ClaimsService(
        IMapper mapper,
        IClaimsRepository repository,
        ICoversRepository coversRepository,
        IBus bus)
    {
        _mapper = mapper;
        _repository = repository;
        _coversRepository = coversRepository;
        _bus = bus;
    }

    public async Task<Result<ClaimDto>> CreateAsync(CreateClaimDto request, CancellationToken cancellationToken = default)
    {
        var validationResponse = await ValidateAsync(request, cancellationToken);
        if (validationResponse.IsFailed)
            return Result.Fail(validationResponse.Errors);
        
        var type = _mapper.Map<ClaimType>(request.Type);
        var claim = Claim.Create(request.CoverId, request.Name, type, request.DamageCost, request.Created);
        
        var entity = _mapper.Map<ClaimEntity>(claim);
        var result = await _repository.CreateAsync(entity, cancellationToken);

        await _bus.Publish(new ClaimAuditEvent(entity.Id, HttpRequestType.POST), cancellationToken);

        return Result.Ok(_mapper.Map<ClaimDto>(result));
    }

    public async Task<Result<ClaimDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(id, cancellationToken);
        if (result == default)
            return Result.Fail(new NotFoundError());

        return Result.Ok(_mapper.Map<ClaimDto>(result));
    }

    public async Task<Result<IEnumerable<ClaimDto>>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetListAsync(cancellationToken);

        return Result.Ok(_mapper.Map<IEnumerable<ClaimDto>>(result));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        
        await _bus.Publish(new ClaimAuditEvent(id, HttpRequestType.DELETE), cancellationToken);

        return Result.Ok();
    }

    private async Task<Result> ValidateAsync(CreateClaimDto request, CancellationToken cancellationToken = default)
    {
        var cover = await _coversRepository.GetAsync(request.CoverId, cancellationToken);
        if (cover == default)
            return Result.Fail(new NotFoundError());

        if (DateOnly.FromDateTime(cover.StartDate) > request.Created || request.Created > DateOnly.FromDateTime(cover.EndDate))
            return Result.Fail(new BadRequestError());

        return Result.Ok();
    }
}