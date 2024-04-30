using Claims.Infrastructure.Persistence.Common;
using Claims.Infrastructure.Persistence.Entities;
using Claims.Infrastructure.Persistence.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            // TODO investigate why none of HasConversion<string>() did not work
            .HasConversion(new EnumToStringConverter<HttpRequestType>())  
            // .HasConversion(
            //     status => status.ToString(),
            //     value => (HttpRequestType)Enum.Parse(typeof(HttpRequestType), value)
            // )
            .HasMaxLength(EntityConfiguration.MaxLengthConstants.HttpRequestType)
            .IsRequired();
    }
}
