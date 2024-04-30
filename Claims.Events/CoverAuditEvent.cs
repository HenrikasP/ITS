using Claims.Events.Enums;

namespace Claims.Events;

public class CoverAuditEvent
{
    public Guid CoverId { get; set; }
    public DateTimeOffset Created { get; init; }
    public HttpRequestType HttpRequestType { get; set; }

    public CoverAuditEvent()
    {
    }

    public CoverAuditEvent(Guid coverId, HttpRequestType type, DateTimeOffset created)
    {
        CoverId = coverId;
        HttpRequestType = type;
        Created = created;
    }

    public CoverAuditEvent(Guid coverId, HttpRequestType type)
    {
        CoverId = coverId;
        HttpRequestType = type;
        Created = DateTimeOffset.Now;
    }
}