using HappyTravel.StdOutLogger.Extensions;
using HappyTravel.StdOutLogger.Infrastructure;
using HappyTravel.Wakayama.Api.Infrastructure.Helpers;

namespace HappyTravel.Wakayama.Api.Infrastructure.Extensions;

public static class LoggingConfigurationExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders()
            .AddConfiguration(builder.Configuration.GetSection("Logging"));
        
        if (builder.Environment.IsLocal())
            builder.Logging.AddConsole();
        else
        {
            builder.Logging.AddStdOutLogger(setup =>
            {
                setup.IncludeScopes = true;
                setup.RequestIdHeader = Constants.DefaultRequestIdHeader;
                setup.UseUtcTimestamp = true;
            });
            builder.Logging.AddSentry();
        }
    }
}