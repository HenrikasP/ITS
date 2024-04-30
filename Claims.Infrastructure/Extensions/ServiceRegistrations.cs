using Claims.Infrastructure.Persistence;
using Claims.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Claims.Infrastructure.Extensions;

public static class ServiceRegistrations
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddScoped<ICoversRepository, CoversRepository>();
        services.AddScoped<IClaimsRepository, ClaimsRepository>();
        
        services.AddDbContext<AuditContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddDbContext<ClaimsContext>(
            options =>
            {
                var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
                var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
                options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
            }
        );

        return services;
    }

    public static void MigrateDatabase(this IServiceProvider serviceProvider)
    {
        var candidateDb = serviceProvider
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<AuditContext>();

        candidateDb.Database.Migrate();
    }
}
