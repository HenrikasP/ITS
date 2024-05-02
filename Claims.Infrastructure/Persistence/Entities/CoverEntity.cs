using Claims.Infrastructure.Persistence.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Infrastructure.Persistence.Entities;

public class CoverEntity
{
    [BsonId]
    public Guid Id { get; set; }

    [BsonElement("startDate")]
    // in 8.0.0 version is currently not supported
    // [BsonDateTimeOptions(DateOnly = true)]
    public DateTime StartDate { get; set; }

    [BsonElement("endDate")]
    // in 8.0.0 version is currently not supported
    // [BsonDateTimeOptions(DateOnly = true)]
    public DateTime EndDate { get; set; }

    [BsonElement("claimType")]
    public CoverType Type { get; set; }

    [BsonElement("premium")]
    public decimal Premium { get; set; }
}