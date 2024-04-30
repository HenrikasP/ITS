using System.Reflection;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Claims.Application.Consumers;
using Claims.Application.Extensions;
using Claims.Application.Mappings;
using Claims.Contracts.Requests;
using Claims.Contracts.Responses;
using Claims.Filters;
using Claims.Infrastructure.Extensions;
using Claims.Infrastructure.Mappings;
using Claims.Options;
using Claims.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using MassTransit;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Claims.Capabilities;

public static class StartupInjection
{
    public static IServiceCollection ConfigureInjection(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddApplication(configuration);
        services.AddInfrastructure(configuration);
        services.ConfigureResultTransformations();
        
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(Program).Assembly, typeof(ApplicationMappings).Assembly, typeof(InfrastructureMappings).Assembly);
        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);
        
        services.ConfigureFluentValidation();
        services.AddRabbitMQWithConsumers(configuration);

        return services;
    }

    public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Claims - V1",
                    Version = "v1",
                    Description = "Claims service",
                }
            );

            var documentationNames = new[]
            {
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
                $"{typeof(ClaimResponse).Assembly.GetName().Name}.xml"
            };
    
            foreach (var documentationFileName in documentationNames)
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, documentationFileName);
                c.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static IServiceCollection AddApiVersioningWithExplorer(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    public static IMvcBuilder AddControllerOptions(this IMvcBuilder builder)
    {
        builder
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        return builder;
    }
    
    private static IServiceCollection ConfigureResultTransformations(this IServiceCollection services)
    {
        services.AddScoped<IChainService>((_) => new ChainService(
            new NotFoundErrorFilter(),
            new BadRequestErrorFilter()));
        
        return services;
    }
    
    private static void ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddTransient<IValidator<CreateClaimRequest>, CreateClaimRequestValidator>();
        services.AddTransient<IValidator<CreateCoverRequest>, CreateCoverRequestValidator>();
    }
    
    private static IServiceCollection AddRabbitMQWithConsumers(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddOptions<RabbitMQOptions>()
            .BindConfiguration(RabbitMQOptions.ConfigurationKey)
            .ValidateDataAnnotations();
        
        services
            .AddMassTransit(x =>
            {
                var options = configuration.GetSection(RabbitMQOptions.ConfigurationKey).Get<RabbitMQOptions>()!;
                x.AddConsumersFromNamespaceContaining<ClaimAuditEventConsumer>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(options.Prefix, false));

                // Setup RabbitMQ Endpoint
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options.Host, options.VirtualHost, host =>
                    {
                        host.Username(options.Username);
                        host.Password(options.Password);
                    });
                    
                    cfg.ConfigureEndpoints(context);
                });
            });

        return services;
    }
    
    public static WebApplicationBuilder SetupSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}