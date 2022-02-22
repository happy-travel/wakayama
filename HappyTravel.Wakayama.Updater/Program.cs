using HappyTravel.Wakayama.Common.ElasticClients;
using HappyTravel.Wakayama.Common.Extensions;
using HappyTravel.Wakayama.Common.HealthChecks;
using HappyTravel.Wakayama.Updater.Infrastracture.Extensions;
using HappyTravel.Wakayama.Updater.Infrastracture.Helpers;
using HappyTravel.Wakayama.Updater.Services;
using HappyTravel.Wakayama.Updater.Services.ElasticClients;

var builder = WebApplication.CreateBuilder(args);

using var vaultClient = VaultClientHelper.GetVaultClient(builder.Configuration);
builder.ConfigureApp();
builder.ConfigureLogging();
builder.ConfigureServiceProvider();

builder.ConfigureElasticClient(vaultClient);
builder.ConfigurePhotonImporter(vaultClient);

builder.Services.AddHealthChecks()
    .AddCheck<ElasticHealthCheck>("Elastic");

builder.Services.AddSingleton<GeoServiceElasticClient>();
builder.Services.AddSingleton<PhotonElasticClient>();
builder.Services.AddSingleton<IPlacesImporter, PhotonDataImporter>();
builder.Services.AddHostedService<ImportPlacesInvoker>();

var app = builder.Build();
app.Run();