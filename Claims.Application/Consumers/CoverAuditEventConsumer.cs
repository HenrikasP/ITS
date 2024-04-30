using Claims.Application.Services.Auditing;
using Claims.Events;
using Claims.Infrastructure.Persistence.Entities.Enums;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Claims.Application.Consumers;

public class CoverAuditEventConsumer : IConsumer<CoverAuditEvent>
{
    private readonly IMapper _mapper;
    private readonly IAuditingService _auditingService;
    private readonly ILogger<CoverAuditEventConsumer> _logger;

    public CoverAuditEventConsumer(
        IMapper mapper,
        IAuditingService auditingService,
        ILogger<CoverAuditEventConsumer> logger)
    {
        _mapper = mapper;
        _auditingService = auditingService;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<CoverAuditEvent> auditEvent)
    {
        var payload = auditEvent.Message;
        _logger.LogDebug($"Consuming {nameof(CoverAuditEvent)} {{HttpRequestType}}", payload.HttpRequestType);

        var type = _mapper.Map<HttpRequestType>(payload.HttpRequestType);
        await _auditingService.AuditCoverAsync(payload.CoverId, type, payload.Created);
    }
}