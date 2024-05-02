using Claims.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistence;

public class AuditContext : BaseContext<AuditContext>
{
    public AuditContext(DbContextOptions<AuditContext> options) : base(options)
    {
    }

    public DbSet<ClaimAuditEntity> ClaimAudits { get; set; }
    public DbSet<CoverAuditEntity> CoverAudits { get; set; }
}