using Claims.Events.Enums;

namespace Claims.Events;

public class ClaimAuditEvent
{
    public Guid ClaimId { get; set; }
    public DateTimeOffset Created { get; set; }
    public HttpRequestType HttpRequestType { get; set; }
    
    public ClaimAuditEvent()
    {
    }
    
    public ClaimAuditEvent(Guid claimId, HttpRequestType type, DateTimeOffset created)
    {
        ClaimId = claimId;
        HttpRequestType = type;
        Created = created;
    }
    
    public ClaimAuditEvent(Guid claimId, HttpRequestType type)
    {
        ClaimId = claimId;
        HttpRequestType = type;
        Created = DateTimeOffset.Now;
    }
}