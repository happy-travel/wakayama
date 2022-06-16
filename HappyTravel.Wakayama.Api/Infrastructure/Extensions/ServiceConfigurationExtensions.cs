using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using HappyTravel.ErrorHandling.Extensions;
using HappyTravel.LocationNameNormalizer.Extensions;
using HappyTravel.Telemetry.Extensions;
using HappyTravel.VaultClient;
using HappyTravel.Wakayama.Api.Infrastructure.Options;
using HappyTravel.Wakayama.Api.Services;
using HappyTravel.Wakayama.Common.ElasticClients;
using HappyTravel.Wakayama.Common.Extensions;
using HappyTravel.Wakayama.Common.Helpers;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace HappyTravel.Wakayama.Api.Infrastructure.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        using var vaultClient = new VaultClient.VaultClient(new VaultOptions
        {
            BaseUrl = new Uri(EnvironmentVariableHelper.Get("Vault:Endpoint", configuration)),
            Engine = builder.Configuration["Vault:Engine"],
            Role = builder.Configuration["Vault:Role"]
        });
        
        vaultClient.Login(EnvironmentVariableHelper.Get("Vault:Token", builder.Configuration)).GetAwaiter().GetResult();
        
        builder.ConfigureElasticClient(vaultClient);

        builder.Services
            .AddMvcCore()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        builder.Services
            .AddProblemDetailsErrorHandling()
            .AddHttpContextAccessor()
            .ConfigureApiVersioning()
            .AddEndpointsApiExplorer()
            .ConfigureSwagger()
            .ConfigureAuthentication(builder.Configuration)
            .AddAuthorization()
            .AddTracing(builder.Configuration, options =>
            {
                options.ServiceName = $"{builder.Environment.ApplicationName}-{builder.Environment.EnvironmentName}";
                options.JaegerHost = builder.Environment.IsLocal()
                    ? builder.Configuration.GetValue<string>("Jaeger:AgentHost")
                    : builder.Configuration.GetValue<string>(
                        builder.Configuration.GetValue<string>("Jaeger:AgentHost"));
                options.JaegerPort = builder.Environment.IsLocal()
                    ? builder.Configuration.GetValue<int>("Jaeger:AgentPort")
                    : builder.Configuration.GetValue<int>(builder.Configuration.GetValue<string>("Jaeger:AgentPort"));
            })
            .AddNameNormalizationServices()
            .AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.ImplicitlyValidateRootCollectionElements = true;
                fv.ImplicitlyValidateChildProperties = true;
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            })
            .AddHealthChecks();


        builder.Services
            .AddSingleton<ElasticGeoServiceClient>()
            .AddTransient<IReverseGeocodingService, ReverseGeocodingService>()
            .AddTransient<IPlaceService, PlaceService>()
            .AddTransient<ResponseBuilder>();
    }
    
    
    public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services) 
        => services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = false;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });
    
    
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services) 
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HappyTravel.Wakayama.Api", Version = "v1" });
                
            var xmlCommentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
            c.IncludeXmlComments(xmlCommentsFilePath);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    Array.Empty<string>()
                }
            });
        });
    
        
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authorityOptions = configuration.GetSection("Authority").Get<AuthorityOptions>();
            
        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authorityOptions.AuthorityUrl;
                options.Audience = authorityOptions.Audience;
                options.RequireHttpsMetadata = true;
                options.AutomaticRefreshInterval = authorityOptions.AutomaticRefreshInterval;
            });

        return services;
    }
}
