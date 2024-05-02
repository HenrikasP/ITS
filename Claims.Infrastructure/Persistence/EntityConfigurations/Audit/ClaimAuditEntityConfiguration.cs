using Claims.Infrastructure.Persistence.Common;
using Claims.Infrastructure.Persistence.Entities;
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
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(c => c.ClaimId)
            .IsRequired();
        
        builder.Property(j => j.Created)
            .IsRequired();
        
        builder.Property(c => c.HttpRequestType)
            .HasConversion<string>()
            .HasMaxLength(EntityConfiguration.MaxLengthConstants.HttpRequestType)
            .IsRequired();
    }
}
