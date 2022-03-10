using HappyTravel.ConsulKeyValueClient.ConfigurationProvider.Extensions;
using HappyTravel.Wakayama.Common.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace HappyTravel.Wakayama.Common.Extensions;

public static class AppConfigurationExtensions
{
    public static void ConfigureApp(this WebApplicationBuilder builder, string consulKey)
    {
        var environment = builder.Environment;
        var consulAddress = Environment.GetEnvironmentVariable("CONSUL_HTTP_ADDR");
        var consulToken = Environment.GetEnvironmentVariable("CONSUL_HTTP_TOKEN");

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddConsulKeyValueClient(consulAddress,
                consulKey,
                consulToken,
                environment.EnvironmentName,
                environment.IsLocal())
            .AddEnvironmentVariables();
    }
}