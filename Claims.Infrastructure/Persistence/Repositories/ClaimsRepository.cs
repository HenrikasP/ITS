using Claims.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistence.Repositories;

public class ClaimsRepository : IClaimsRepository
{
    private readonly ClaimsContext _context;

    public ClaimsRepository(ClaimsContext context)
    {
        _context = context;
    }

    public async Task<ClaimEntity> CreateAsync(ClaimEntity item, CancellationToken cancellationToken = default)
    {
        var result = _context.Claims.Add(item);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task<ClaimEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Claims
            .Where(claim => claim.Id == id)
            .SingleOrDefaultAsync(cancellationToken);;
    }

    public async Task<IEnumerable<ClaimEntity>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Claims.ToListAsync(cancellationToken);;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var claim = await GetAsync(id, cancellationToken);
        if (claim is not null)
        {
            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
