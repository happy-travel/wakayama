using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace HappyTravel.Wakayama.Common.Extensions;

public static class SentryConfigurationExtensions
{
    public static void ConfigureSentry(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = Environment.GetEnvironmentVariable("HTDC_WAKAYAMA_SENTRY_ENDPOINT");
            options.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
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