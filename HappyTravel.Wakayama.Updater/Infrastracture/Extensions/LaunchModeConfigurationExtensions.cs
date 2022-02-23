using HappyTravel.Wakayama.Updater.Options;

namespace HappyTravel.Wakayama.Updater.Infrastracture.Extensions;

public static class LaunchModeConfigurationExtensions
{
    public static void ConfigureUpdaterLaunchSettings(this WebApplicationBuilder builder)
    {
        var launchModeValue = builder.Configuration.GetValue<string>("LaunchOptions:LaunchMode");
        builder.Services.Configure<UpdaterOptions>(o =>
        {
            o.LaunchMode = Enum.Parse<LaunchMode>(launchModeValue);
        });
    }
}