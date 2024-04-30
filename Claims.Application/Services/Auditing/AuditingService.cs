using Claims.Infrastructure.Persistence;
using Claims.Infrastructure.Persistence.Entities;
using Claims.Infrastructure.Persistence.Entities.Enums;

namespace Claims.Application.Services.Auditing;

public class AuditingService : IAuditingService
{
    private readonly AuditContext _auditContext;

    public AuditingService(AuditContext auditContext)
    {
        _auditContext = auditContext;
    }

    public Task AuditClaimAsync(Guid id, HttpRequestType type, DateTimeOffset created, CancellationToken cancellationToken = default)
    {
        var claimAudit = new ClaimAuditEntity
        {
            ClaimId = id,
            Created = created,
            HttpRequestType = type
        };
        
        _auditContext.Add(claimAudit);
        return _auditContext.SaveChangesAsync(cancellationToken);
    }

    public Task AuditCoverAsync(Guid id, HttpRequestType type, DateTimeOffset created, CancellationToken cancellationToken = default)
    {
        var coverAudit = new CoverAuditEntity
        {
            CoverId = id,
            Created = created,
            HttpRequestType = type
        };

        _auditContext.Add(coverAudit);
        return _auditContext.SaveChangesAsync(cancellationToken);
    }
}