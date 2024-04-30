using Claims.Application.Models;
using FluentResults;

namespace Claims.Application.Services.Covers;

public interface ICoversService
{
    Task<Result<CoverDto>> CreateAsync(CreateCoverDto cover, CancellationToken cancellationToken = default);
    Task<Result<CoverDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CoverDto>>> GetListAsync(CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}