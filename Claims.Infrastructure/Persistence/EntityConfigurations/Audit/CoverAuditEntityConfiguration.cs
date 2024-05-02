using Claims.Infrastructure.Persistence.Common;
using Claims.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Claims.Infrastructure.Persistence.EntityConfigurations.Audit;

public class CoverAuditEntityConfiguration : IEntityTypeConfiguration<CoverAuditEntity>
{
    public void Configure(EntityTypeBuilder<CoverAuditEntity> builder)
    {
        builder.ToTable(Schemas.Tables.CoverAudit, Schemas.Audit);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(c => c.CoverId)
            .IsRequired();
        
        builder.Property(j => j.Created)
            .IsRequired();
        
        builder.Property(c => c.HttpRequestType)
            .HasConversion<string>()
            .HasMaxLength(EntityConfiguration.MaxLengthConstants.HttpRequestType)
            .IsRequired();
    }
}
