using Claims.Application.Models;
using Claims.Infrastructure.Persistence.Entities;
using Mapster;

namespace Claims.Application.Mappings;

public class ApplicationMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
        
        config.NewConfig<CoverEntity, CoverDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.StartDate, src => DateOnly.FromDateTime(src.StartDate))
            .Map(dest => dest.EndDate, src => DateOnly.FromDateTime(src.EndDate))
            .Map(dest => dest.Premium, src => src.Premium);

        config.NewConfig<ClaimEntity, ClaimDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CoverId, src => src.CoverId)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Created, src => DateOnly.FromDateTime(src.Created))
            .Map(dest => dest.DamageCost, src => src.DamageCost)
            .Map(dest => dest.Name, src => src.Name);
    }
}