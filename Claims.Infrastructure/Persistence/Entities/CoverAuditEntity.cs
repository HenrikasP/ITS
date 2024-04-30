using Claims.Infrastructure.Persistence.Entities.Enums;

namespace Claims.Infrastructure.Persistence.Entities;

public class CoverAuditEntity
{
    public int Id { get; set; }

    public Guid CoverId { get; set; }

    public DateTimeOffset Created { get; set; }

    public HttpRequestType HttpRequestType { get; set; }
}