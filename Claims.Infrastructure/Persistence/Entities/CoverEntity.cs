using Claims.Infrastructure.Persistence.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Infrastructure.Persistence.Entities;

public class CoverEntity
{
    [BsonId]
    public Guid Id { get; set; }

    [BsonElement("startDate")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime StartDate { get; set; }

    [BsonElement("endDate")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime EndDate { get; set; }

    [BsonElement("claimType")]
    public CoverType Type { get; set; }

    [BsonElement("premium")]
    public decimal Premium { get; set; }
}