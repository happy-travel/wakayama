using System.Security.Cryptography.X509Certificates;
using HappyTravel.VaultClient;
using HappyTravel.Wakayama.Common.Helpers;
using HappyTravel.Wakayama.Updater.Options;
using Nest;

namespace HappyTravel.Wakayama.Updater.Infrastracture.Extensions;

public static class ElasticImporterConfigurations
{
    public static IServiceCollection ConfigurePhotonImporter(this WebApplicationBuilder builder, IVaultClient vaultClient)
    {
        var serviceCollection = builder.Services;
        var configuration = builder.Configuration;
        var environment = builder.Environment;
        ConnectionSettings connectionSettings;
        string indexName;
        int top;
        if (environment.IsLocal())
        {
            connectionSettings = GetPhotonLocalConnectionsSettings(configuration);
            indexName = configuration["Importers:Photon:Settings:Index"];
            top = int.Parse(configuration["Importers:Photon:Settings:Top"]);
        }
        else
        {
            var photonSettingsDictionary = vaultClient.Get(configuration["Importers:Photon:Settings"]).GetAwaiter().GetResult();
            indexName = photonSettingsDictionary["index"];
            top = int.Parse(photonSettingsDictionary["top"]);
            var requestTimeoutInSeconds = int.Parse(configuration["requestTimeoutInSeconds"]);
            connectionSettings = new ConnectionSettings(new Uri(photonSettingsDictionary["endpoint"]))
                .BasicAuthentication(photonSettingsDictionary["username"], photonSettingsDictionary["password"])
                .ServerCertificateValidationCallback((o, certificate, chain, errors) => true)
                .ClientCertificate(new X509Certificate2(Convert.FromBase64String(photonSettingsDictionary["certificate"])))
                .RequestTimeout(TimeSpan.FromSeconds(requestTimeoutInSeconds));
            
        }
        SetDefaultIndex(connectionSettings, indexName);

        builder.Services.Configure<PhotonImporterOptions>(o =>
        {
            o.ConnectionSettings = connectionSettings;
            o.Index = indexName;
            o.Top = top;
        });
        
        return serviceCollection;
    }

    private static ConnectionSettings GetPhotonLocalConnectionsSettings(IConfiguration configuration)
        => new ConnectionSettings(new Uri(configuration["Importers:Photon:Settings:Endpoint"]))
            .DefaultIndex(configuration["Importers:Photon:Settings:Index"])
            .BasicAuthentication(configuration["Importers:Photon:Settings:Username"], 
                configuration["Importers:Photon:Settings:Password"])
            .RequestTimeout(TimeSpan.FromSeconds(int.Parse(configuration["Importers:Photon:Settings:RequestTimeoutInSeconds"])));


    private static void SetDefaultIndex(ConnectionSettings connectionSettings, string index) 
        => connectionSettings.DefaultIndex(index);
}