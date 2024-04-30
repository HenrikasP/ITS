using Claims.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistence;

public class ClaimsContext : BaseContext<ClaimsContext>
{
    public DbSet<ClaimEntity> Claims { get; init; }
    public DbSet<CoverEntity> Covers { get; init; }

    public ClaimsContext(DbContextOptions<ClaimsContext> options)
        : base(options)
    {
        Claims = Set<ClaimEntity>();
        Covers = Set<CoverEntity>();
    }
}