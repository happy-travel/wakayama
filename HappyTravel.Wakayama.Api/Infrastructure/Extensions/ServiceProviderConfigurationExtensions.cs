namespace HappyTravel.Wakayama.Api.Infrastructure.Extensions;

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