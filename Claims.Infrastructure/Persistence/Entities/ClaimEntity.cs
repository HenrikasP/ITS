using Claims.Infrastructure.Persistence.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Infrastructure.Persistence.Entities;

public class ClaimEntity
{
    [BsonId]
    public Guid Id { get; set; }

    [BsonElement("coverId")]
    public Guid CoverId { get; set; }

    [BsonElement("created")]
    // in 8.0.0 version is currently not supported
    // [BsonDateTimeOptions(DateOnly = true)]
    public DateTime Created { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("claimType")]
    public ClaimType Type { get; set; }

    [BsonElement("damageCost")]
    public decimal DamageCost { get; set; }
}