using Claims.Infrastructure.Persistence.Entities.Enums;

namespace Claims.Application.Services.Auditing;

public interface IAuditingService
{
    Task AuditClaimAsync(Guid id, HttpRequestType type, DateTimeOffset created, CancellationToken cancellationToken = default);
    Task AuditCoverAsync(Guid id, HttpRequestType type, DateTimeOffset created, CancellationToken cancellationToken = default);
}