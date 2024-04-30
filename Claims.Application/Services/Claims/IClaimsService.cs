using Claims.Application.Models;
using FluentResults;

namespace Claims.Application.Services.Claims;

public interface IClaimsService
{
    Task<Result<ClaimDto>> CreateAsync(CreateClaimDto claim, CancellationToken cancellationToken = default);
    Task<Result<ClaimDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ClaimDto>>> GetListAsync(CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}