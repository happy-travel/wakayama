using System.Security.Cryptography.X509Certificates;
using HappyTravel.VaultClient;
using HappyTravel.Wakayama.Common.Helpers;
using HappyTravel.Wakayama.Updater.Options;
using Nest;

namespace HappyTravel.Wakayama.Updater.Infrastracture.Extensions;

public static class PhotonDataUpdaterConfigurationExtensions
{
    public static IServiceCollection ConfigurePhotonDataUpdater(this WebApplicationBuilder builder, IVaultClient vaultClient)
    {
        var serviceCollection = builder.Services;
        var configuration = builder.Configuration;
        var environment = builder.Environment;
        ConnectionSettings connectionSettings;
        string indexName;

        if (environment.IsLocal())
        {
            connectionSettings = new ConnectionSettings(new Uri(configuration["Updaters:Photon:Settings:Endpoint"]))
                .DefaultIndex(configuration["Updaters:Photon:Settings:Index"])
                .BasicAuthentication(configuration["Updaters:Photon:Settings:Username"], 
                    configuration["Updaters:Photon:Settings:Password"])
                .RequestTimeout(TimeSpan.FromSeconds(int.Parse(configuration["Updaters:Photon:Settings:RequestTimeoutInSeconds"])));
            indexName = configuration["Updaters:Photon:Settings:Index"];
        }
        else
        {
            var photonSettingsDictionary = vaultClient.Get(configuration["Updaters:Photon:Settings"]).GetAwaiter().GetResult();
            indexName = photonSettingsDictionary["index"];
            var requestTimeoutInSeconds = int.Parse(photonSettingsDictionary["requestTimeoutInSeconds"]);
            connectionSettings = new ConnectionSettings(new Uri(photonSettingsDictionary["endpoint"]))
                .BasicAuthentication(photonSettingsDictionary["username"], photonSettingsDictionary["password"])
                .ServerCertificateValidationCallback((o, certificate, chain, errors) => true)
                .ClientCertificate(new X509Certificate2(Convert.FromBase64String(photonSettingsDictionary["certificate"])))
                .RequestTimeout(TimeSpan.FromSeconds(requestTimeoutInSeconds));
        }
        SetDefaultIndex(connectionSettings, indexName);

        builder.Services.Configure<PhotonUpdaterOptions>(o =>
        {
            o.ConnectionSettings = connectionSettings;
            o.Index = indexName;
        });
        
        return serviceCollection;
    }


    private static void SetDefaultIndex(ConnectionSettings connectionSettings, string index) 
        => connectionSettings.DefaultIndex(index);
}