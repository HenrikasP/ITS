using Asp.Versioning;
using Claims.Application.Models;
using Claims.Application.Services.Covers;
using Claims.Application.Services.PremiumCalculation;
using Claims.Contracts.Requests;
using Claims.Contracts.Responses;
using Claims.Domain.Aggregates;
using Claims.Filters;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using CoverType = Claims.Application.Models.Enums.CoverType;

namespace Claims.Controllers;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class CoversController : DocumentedControllerBase
{
    private readonly ICoversService _coversService;
    private readonly IMapper _mapper;
    private readonly IPremiumCalculationService _premiumCalculationService;
    private readonly IChainService _chainService;
    private readonly ILogger<CoversController> _logger;

    public CoversController(
        IMapper mapper,
        ICoversService coversService,
        IPremiumCalculationService premiumCalculationService,
        IChainService chainService,
        ILogger<CoversController> logger)
    {
        _coversService = coversService;
        _mapper = mapper;
        _premiumCalculationService = premiumCalculationService;
        _chainService = chainService;
        _logger = logger;
    }
    
    /// <summary>
    /// You can Compute premium here
    /// </summary>
    /// <remarks>
    ///  Premium depends on the type of the covered object and the length of the insurance period.
    ///  *   Base day rate was set to be 1250.
    ///  *   Yacht should be 10% more expensive, Passenger ship 20%, Tanker 50%, and other types 30%
    ///  *   The length of the insurance period should influence the premium progressively:
    ///  *   First 30 days are computed based on the logic above
    ///  *   Following 150 days are discounted by 5% for Yacht and by 2% for other types
    ///  *   The remaining days are discounted by additional 3% for Yacht and by 1% for other types
    ///  
    /// Sample request:
    ///     
    ///     POST /v1/claims
    ///     {
    ///         "startDate": "2024-01-01",
    ///         "endDate": "2024-06-01",
    ///         "coverType": "Yacht"
    ///     }
    /// </remarks>
    /// <param name="request"></param>
    /// <returns> This endpoint returns a list of Accounts.</returns>
    [HttpPost("compute")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public ActionResult<decimal> ComputePremium([FromBody] ComputePremiumRequest request)
    {
        var type = _mapper.Map<CoverType>(request.CoverType);
        var result = _premiumCalculationService.ComputePremium(request.StartDate, request.EndDate, type);
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CoverResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<CoverResponse>> CreateAsync([FromBody] CreateCoverRequest cover, CancellationToken cancellationToken = default)
    {
        var request = _mapper.Map<CreateCoverDto>(cover);
        var result = await _coversService.CreateAsync(request, cancellationToken);
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);

        return Created($"{result.Value.Id}", _mapper.Map<CoverResponse>(result.Value));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CoverResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Cover>>> GetListAsync(CancellationToken cancellationToken = default)
    {
        // TODO pagination
        var result = await _coversService.GetListAsync(cancellationToken);
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);

        return Ok(_mapper.Map<IEnumerable<CoverResponse>>(result.Value));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IEnumerable<CoverResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Cover>> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _coversService.GetAsync(id, cancellationToken);
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);

        return Ok(_mapper.Map<CoverResponse>(result.Value));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _coversService.DeleteAsync(id, cancellationToken);
        if (result.IsFailed)
            return _chainService.Execute(result.Errors);

        return NoContent();
    }
}
