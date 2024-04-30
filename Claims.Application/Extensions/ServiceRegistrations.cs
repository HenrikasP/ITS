using Claims.Application.Services.Auditing;
using Claims.Application.Services.Claims;
using Claims.Application.Services.Covers;
using Claims.Application.Services.PremiumCalculation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Application.Extensions;

public static class ServiceRegistrations
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddScoped<ICoversService, CoversService>();
        services.AddScoped<IPremiumCalculationService, PremiumCalculationService>();
        services.AddScoped<IClaimsService, ClaimsService>();
        
        services.AddScoped<IAuditingService, AuditingService>();

        return services;
    }
}
