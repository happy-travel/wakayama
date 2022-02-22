using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace HappyTravel.Wakayama.Common.Extensions;

public static class AppConfigurationExtensions
{
    public static void ConfigureApp(this WebApplicationBuilder builder)
    {
        var environment = builder.Environment;

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}