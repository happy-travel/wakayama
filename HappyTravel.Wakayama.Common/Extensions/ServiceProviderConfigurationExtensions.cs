using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace HappyTravel.Wakayama.Common.Extensions;

public static class ServiceProviderConfigurationExtensions
{
    public static void ConfigureServiceProvider(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseDefaultServiceProvider(o =>
        {
            o.ValidateScopes = true;
            o.ValidateOnBuild = true;
        });
    }
}