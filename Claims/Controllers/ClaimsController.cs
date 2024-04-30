using Asp.Versioning;
using Claims.Application.Errors;
using Claims.Application.Models;
using Claims.Application.Services.Claims;
using Claims.Contracts.Requests;
using Claims.Contracts.Responses;
using Claims.Domain.Aggregates;
using Claims.Filters;
using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("v{version:apiVersion}/[controller]")]
public class ClaimsController : DocumentedControllerBase
{
    private readonly IClaimsService _claimsService;
    private readonly IChainService _chainService;
    private readonly IMapper _mapper;

    public ClaimsController(
        IClaimsService claimsService,
        IChainService chainService,
        IMapper mapper)
    {
        _claimsService = claimsService;
        _chainService = chainService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClaimResponse), 200)]
    public async Task<ActionResult<ClaimResponse>> CreateAsync(CreateClaimRequest claim, CancellationToken cancellationToken = default)
    {
        var request = _mapper.Map<CreateClaimDto>(claim);
        var result = await _claimsService.CreateAsync(request, cancellationToken);
        result.Errors.ToResult().HasError<NotFoundError>();
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);
    
        return Created($"v1/{result.Value.Id}", _mapper.Map<ClaimResponse>(result.Value));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClaimResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClaimResponse>>> GetListAsync(CancellationToken cancellationToken = default)
    {
        // TODO pagination
        var result = await _claimsService.GetListAsync(cancellationToken);
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);

        return Ok(_mapper.Map<IEnumerable<ClaimResponse>>(result.Value));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClaimResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<Claim>> GetAsync([FromRoute]Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _claimsService.GetAsync(id, cancellationToken);
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);

        return Ok(_mapper.Map<ClaimResponse>(result.Value));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _claimsService.DeleteAsync(id, cancellationToken);
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);

        return NoContent();
    }
}