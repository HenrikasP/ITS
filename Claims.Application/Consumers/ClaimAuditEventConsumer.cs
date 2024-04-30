using Claims.Application.Services.Auditing;
using Claims.Events;
using Claims.Infrastructure.Persistence.Entities.Enums;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Claims.Application.Consumers;

public class ClaimAuditEventConsumer : IConsumer<ClaimAuditEvent>
{
    private readonly IMapper _mapper;
    private readonly IAuditingService _auditingService;
    private readonly ILogger<ClaimAuditEventConsumer> _logger;

    public ClaimAuditEventConsumer(
        IMapper mapper,
        IAuditingService auditingService,
        ILogger<ClaimAuditEventConsumer> logger)
    {
        _mapper = mapper;
        _auditingService = auditingService;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<ClaimAuditEvent> auditEvent)
    {
        var payload = auditEvent.Message;
        _logger.LogDebug($"Consuming {nameof(ClaimAuditEvent)} {{HttpRequestType}}", payload.HttpRequestType);

        var type = _mapper.Map<HttpRequestType>(payload.HttpRequestType);
        await _auditingService.AuditClaimAsync(payload.ClaimId, type, payload.Created);
    }
}