using System.Reflection;
using HappyTravel.ErrorHandling.Extensions;
using HappyTravel.Telemetry.Extensions;
using HappyTravel.VaultClient;
using HappyTravel.Wakayama.Api.Infrastructure.Helpers;
using HappyTravel.Wakayama.Data;
using HappyTravel.Wakayama.Data.CompiledModels;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace HappyTravel.Wakayama.Api.Infrastructure.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        using var vaultClient = new VaultClient.VaultClient(new VaultOptions
        {
            BaseUrl = new Uri(EnvironmentVariableHelper.Get("Vault:Endpoint", builder.Configuration)),
            Engine = builder.Configuration["Vault:Engine"],
            Role = builder.Configuration["Vault:Role"]
        });
            
        vaultClient.Login(EnvironmentVariableHelper.Get("Vault:Token", builder.Configuration)).GetAwaiter().GetResult();

        builder.Services
            .AddMvcCore()
            .AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.AddPolicy("CanIssue", policy =>
                {
                    policy.RequireClaim("scope", "vcc.issue");
                });
                options.AddPolicy("CanGetHistory", policy =>
                {
                    policy.RequireClaim("scope", "vcc.history");
                });
                options.AddPolicy("CanEdit", policy =>
                {
                    policy.RequireClaim("scope", "vcc.edit");
                });
            })
            .AddApiExplorer();

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<WakayamaContext>();

        builder.Services
            .AddProblemDetailsErrorHandling()
            .AddHttpContextAccessor()
            .ConfigureApiVersioning()
            .ConfigureSwagger()
            .ConfigureDatabaseOptions(vaultClient, builder.Configuration)
            .ConfigureAuthentication(vaultClient, builder.Configuration)
            .AddTracing(builder.Configuration, options =>
            {
                options.ServiceName = $"{builder.Environment.ApplicationName}-{builder.Environment.EnvironmentName}";
                options.JaegerHost = builder.Environment.IsLocal()
                    ? builder.Configuration.GetValue<string>("Jaeger:AgentHost")
                    : builder.Configuration.GetValue<string>(builder.Configuration.GetValue<string>("Jaeger:AgentHost"));
                options.JaegerPort = builder.Environment.IsLocal()
                    ? builder.Configuration.GetValue<int>("Jaeger:AgentPort")
                    : builder.Configuration.GetValue<int>(builder.Configuration.GetValue<string>("Jaeger:AgentPort"));
            });
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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HappyTravel.Gifu.Api", Version = "v1" });
                
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
    
    
    public static IServiceCollection ConfigureDatabaseOptions(this IServiceCollection services, VaultClient.VaultClient vaultClient, 
        IConfiguration configuration)
    {
        var databaseOptions = vaultClient.Get(configuration["Database:Options"]).GetAwaiter().GetResult();
            
        return services.AddDbContextPool<WakayamaContext>(options =>
        {
            var host = databaseOptions["host"];
            var port = databaseOptions["port"];
            var password = databaseOptions["password"];
            var userId = databaseOptions["userId"];
            
            var connectionString = configuration["Database:ConnectionString"];
            options.UseNpgsql(string.Format(connectionString, host, port, userId, password), builder =>
            {
                builder.EnableRetryOnFailure(3);
            });
            options.UseInternalServiceProvider(null);
            options.EnableSensitiveDataLogging(false);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            // run "dotnet ef dbcontext optimize" for regenerating compiled models
            options.UseModel(WakayamaContextModel.Instance);
        }, 16);
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