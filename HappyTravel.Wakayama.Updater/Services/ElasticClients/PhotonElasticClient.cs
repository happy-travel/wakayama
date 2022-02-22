using HappyTravel.Wakayama.Updater.Options;
using Microsoft.Extensions.Options;
using Nest;

namespace HappyTravel.Wakayama.Updater.Services.ElasticClients;

public class PhotonElasticClient
{
    public PhotonElasticClient(IOptions<PhotonImporterOptions> options)
        => Client = new ElasticClient(options.Value.ConnectionSettings);
    

    public IElasticClient Client { get; }
}