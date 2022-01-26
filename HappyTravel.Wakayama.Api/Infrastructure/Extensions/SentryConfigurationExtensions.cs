using System.Diagnostics;

namespace HappyTravel.Wakayama.Api.Infrastructure.Extensions;

public static class SentryConfigurationExtensions
{
    public static void ConfigureSentry(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = System.Environment.GetEnvironmentVariable("HTDC_WAKAYAMA_SENTRY_ENDPOINT");
            options.Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            options.IncludeActivityData = true;
            options.BeforeSend = sentryEvent =>
            {
                if (Activity.Current is null)
                    return sentryEvent;
                                
                foreach (var (key, value) in Activity.Current.Baggage)
                    sentryEvent.SetTag(key, value ?? string.Empty);

                sentryEvent.SetTag("TraceId", Activity.Current.TraceId.ToString());
                sentryEvent.SetTag("SpanId", Activity.Current.SpanId.ToString());

                return sentryEvent;
            };
        });
    }
}