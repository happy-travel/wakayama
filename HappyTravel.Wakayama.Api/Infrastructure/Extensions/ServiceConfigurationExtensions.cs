using System.Reflection;
using HappyTravel.ErrorHandling.Extensions;
using HappyTravel.Telemetry.Extensions;
using HappyTravel.VaultClient;
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

        builder.Services
            .AddMvcCore();


        builder.Services
            .AddProblemDetailsErrorHandling()
            .AddHttpContextAccessor()
            .ConfigureApiVersioning()
            .ConfigureSwagger()
            .ConfigureElastic(vaultClient, configuration)
            .ConfigureAuthentication(vaultClient, builder.Configuration)
            .AddAuthorization()
            .AddTracing(builder.Configuration, options =>
            {
                options.ServiceName = $"{builder.Environment.ApplicationName}-{builder.Environment.EnvironmentName}";
                options.JaegerHost = builder.Environment.IsLocal()
                    ? builder.Configuration.GetValue<string>("Jaeger:AgentHost")
                    : builder.Configuration.GetValue<string>(builder.Configuration.GetValue<string>("Jaeger:AgentHost"));
                options.JaegerPort = builder.Environment.IsLocal()
                    ? builder.Configuration.GetValue<int>("Jaeger:AgentPort")
                    : builder.Configuration.GetValue<int>(builder.Configuration.GetValue<string>("Jaeger:AgentPort"));
            })
            .AddHealthChecks();
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
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
    
    
    public static IServiceCollection ConfigureElastic(this IServiceCollection services, VaultClient.VaultClient vaultClient, IConfiguration configuration)
    {
        return services;
    }
        
        
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IVaultClient vaultClient, IConfiguration configuration)
    {
        var authorityOptions = vaultClient.Get(configuration["AuthorityOptions"]).GetAwaiter().GetResult();
            
        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = authorityOptions["authorityUrl"];
                options.ApiName = authorityOptions["apiName"];
                options.RequireHttpsMetadata = true;
                options.SupportedTokens = SupportedTokens.Jwt;
            });

        return services;
    }
}