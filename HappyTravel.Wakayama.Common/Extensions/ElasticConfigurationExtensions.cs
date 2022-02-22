using System.Security.Cryptography.X509Certificates;
using HappyTravel.Wakayama.Common.Helpers;
using HappyTravel.Wakayama.Common.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace HappyTravel.Wakayama.Common.Extensions;

public static class ElasticConfigurationExtensions
{
    public static WebApplicationBuilder ConfigureElasticClient(this WebApplicationBuilder builder, VaultClient.VaultClient vaultClient)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;
        
        ConnectionSettings clientSettings;
        if (builder.Environment.IsLocal())
        {
            clientSettings = new ConnectionSettings(new Uri(configuration["Elasticsearch:ClientSettings:Endpoint"]))
                    .BasicAuthentication(configuration["Elasticsearch:ClientSettings:Username"], 
                        configuration["Elasticsearch:ClientSettings:Password"])
                    .RequestTimeout(TimeSpan.FromSeconds(int.Parse(configuration["Elasticsearch:ClientSettings:RequestTimeoutInSeconds"])));
        }
        else
        {
            var settingsDictionary = vaultClient.Get(configuration["Elasticsearch:ClientSettings"]).GetAwaiter().GetResult();
            clientSettings = new ConnectionSettings(new Uri(settingsDictionary["endpoint"]))
                    .BasicAuthentication(settingsDictionary["username"], settingsDictionary["password"])
                    .ServerCertificateValidationCallback((o, certificate, chain, errors) => true)
                    .ClientCertificate(new X509Certificate2(Convert.FromBase64String(settingsDictionary["certificate"])))
                    .RequestTimeout(TimeSpan.FromSeconds(int.Parse(settingsDictionary["requestTimeoutInSeconds"])));
        }

        var indexesDictionary  = vaultClient.Get(configuration["Elasticsearch:Indexes"]).GetAwaiter().GetResult();

        services.Configure<ElasticOptions>(o =>
        {
            o.ClientSettings = clientSettings;
            o.Indexes = new()
            {
                HtRelations = indexesDictionary["ht-relations"],
                Places = indexesDictionary["places"]
            };
        });
        
        return builder;
    }
}