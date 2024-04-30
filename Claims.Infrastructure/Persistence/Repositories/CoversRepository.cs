using Claims.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistence.Repositories;

public class CoversRepository : ICoversRepository
{
    private readonly ClaimsContext _context;

    public CoversRepository(ClaimsContext context)
    {
        _context = context;
    }

    public async Task<CoverEntity> CreateAsync(CoverEntity item, CancellationToken cancellationToken = default)
    {
        var result = _context.Covers.Add(item);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task<CoverEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Covers
            .Where(cover => cover.Id == id)
            .SingleOrDefaultAsync(cancellationToken);;
    }

    public async Task<IEnumerable<CoverEntity>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Covers.ToListAsync(cancellationToken);;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cover = await GetAsync(id, cancellationToken);
        if (cover is not null)
        {
            _context.Covers.Remove(cover);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
