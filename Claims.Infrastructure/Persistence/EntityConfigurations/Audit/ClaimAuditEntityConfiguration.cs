using Claims.Infrastructure.Persistence.Common;
using Claims.Infrastructure.Persistence.Entities;
using Claims.Infrastructure.Persistence.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Claims.Infrastructure.Persistence.EntityConfigurations.Audit;

public class ClaimAuditEntityConfiguration : IEntityTypeConfiguration<ClaimAuditEntity>
{
    public void Configure(EntityTypeBuilder<ClaimAuditEntity> builder)
    {
        builder.ToTable(Schemas.Tables.ClaimAudit, Schemas.Audit);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(c => c.ClaimId)
            .IsRequired();
        
        builder.Property(j => j.Created)
            .IsRequired();
        
        builder.Property(c => c.HttpRequestType)
            .HasConversion(
                status => status.ToString(),
                value => (HttpRequestType)Enum.Parse(typeof(HttpRequestType), value)
            )
            .HasMaxLength(EntityConfiguration.MaxLengthConstants.HttpRequestType)
            .IsRequired();
    }
}
