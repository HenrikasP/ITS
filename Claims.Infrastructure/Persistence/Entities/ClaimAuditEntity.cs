using Claims.Infrastructure.Persistence.Entities.Enums;

namespace Claims.Infrastructure.Persistence.Entities;

public class ClaimAuditEntity
{
    public int Id { get; set; }

    public Guid ClaimId { get; set; }

    public DateTimeOffset Created { get; set; }

    public HttpRequestType HttpRequestType { get; set; }
}