using HappyTravel.ConsulKeyValueClient.ConfigurationProvider.Extensions;
using HappyTravel.Wakayama.Api.Infrastructure.Helpers;

namespace HappyTravel.Wakayama.Api.Infrastructure.Extensions;

public static class AppConfigurationExtensions
{
    public static void ConfigureAppConfiguration(this WebApplicationBuilder builder)
    {
        var environment = builder.Environment;
        var consulAddress = Environment.GetEnvironmentVariable("CONSUL_HTTP_ADDR");
        var consulToken = Environment.GetEnvironmentVariable("CONSUL_HTTP_TOKEN");
        
        ArgumentNullException.ThrowIfNull(consulAddress);
        ArgumentNullException.ThrowIfNull(consulToken);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddConsulKeyValueClient( consulAddress,
                "wakayama",
                consulToken,
                environment.EnvironmentName,
                environment.IsLocal());
    }
}