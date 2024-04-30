using Claims.Domain.Aggregates;
using Claims.Infrastructure.Persistence.Entities;
using Mapster;

namespace Claims.Infrastructure.Mappings;

public class InfrastructureMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<Cover, CoverEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.StartDate, src => src.StartDate.ToDateTime(new TimeOnly(0, 0, 0)))
            .Map(dest => dest.EndDate, src => src.EndDate.ToDateTime(new TimeOnly(0, 0, 0)))
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.Premium, src => src.Premium);

        config.NewConfig<CoverEntity, Cover>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.StartDate, src => DateOnly.FromDateTime(src.StartDate))
            .Map(dest => dest.EndDate, src => DateOnly.FromDateTime(src.EndDate))
            .Map(dest => dest.Premium, src => src.Premium);

        config.NewConfig<Claim, ClaimEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CoverId, src => src.CoverId)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Created, src => src.Created.ToDateTime(new TimeOnly(0, 0, 0)))
            .Map(dest => dest.DamageCost, src => src.DamageCost)
            .Map(dest => dest.Name, src => src.Name);

        config.NewConfig<ClaimEntity, Claim>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CoverId, src => src.CoverId)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Created, src => DateOnly.FromDateTime(src.Created))
            .Map(dest => dest.DamageCost, src => src.DamageCost)
            .Map(dest => dest.Name, src => src.Name);
    }
}