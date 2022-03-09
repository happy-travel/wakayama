using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using HappyTravel.Wakayama.Common.ElasticClients;
using HappyTravel.Wakayama.Common.Logging;
using HappyTravel.Wakayama.Common.Models;
using HappyTravel.Wakayama.Common.Options;
using HappyTravel.Wakayama.Updater.Options;
using HappyTravel.Wakayama.Updater.Services.ElasticClients;
using Microsoft.Extensions.Options;
using Nest;

namespace HappyTravel.Wakayama.Updater.Services;

public class PhotonUpdater : IPlacesUpdater
{
    public PhotonUpdater(GeoServiceElasticClient geoServiceElasticClient, PhotonElasticClient photonElasticClient, IOptions<ElasticOptions> elasticOptions, ILogger<PhotonUpdater> logger)
    {
        _geoServiceElasticClient = geoServiceElasticClient;
        _photonElasticClient = photonElasticClient;
        _indexNames = elasticOptions.Value.Indexes;
        _logger = logger;
    }

    
    public async Task Execute(LaunchMode launchMode, CancellationToken cancellationToken)
    {
        try
        {
            await InitializeIndexIfNeeded(launchMode, cancellationToken);
            
            var lastOsmId = await GetLastOsmId();
            
            _logger.LogUpdateStarted(launchMode.ToString(), lastOsmId.ToString());
            
            var totalNumberOfPhotonPlaces = (await GetTotalNumberOfPhotonPlaces(cancellationToken)).Count;
            var numberOfUploadedItems = (await GetTotalNumberOfUploadedPlaces(cancellationToken)).Count;
            
            await foreach (var photonPlaces in DownloadPhotonPlaces(lastOsmId, cancellationToken))
            {
                var uploadResponse = await UploadPlaces(photonPlaces, cancellationToken);
                
                LogUploadErrors(uploadResponse);
                
                numberOfUploadedItems += photonPlaces.Count - uploadResponse.ItemsWithErrors.Count(i => i.Error is not null);
                _logger.LogPlacesUploaded(numberOfUploadedItems, totalNumberOfPhotonPlaces, _indexNames.Places);
            }
        }
        catch (Exception ex)
        {
            _logger.LogUpdateErrorOccurred(ex);
        }

        _logger.LogUpdateCompleted();
    }


    private async Task InitializeIndexIfNeeded(LaunchMode launchMode, CancellationToken cancellationToken)
    {
        if (launchMode == LaunchMode.Undefined)
            throw new InvalidEnumArgumentException("The updater's launch mode is undefined");
        
        if (launchMode == LaunchMode.Full)
        {
            var removeIndexResponse = await _geoServiceElasticClient.RemoveIndex(_indexNames.Places, cancellationToken);
            if (removeIndexResponse.OriginalException is not null)
                throw removeIndexResponse.OriginalException;

            _logger.LogElasticIndexRemoved(_indexNames.Places);
        }

        if (!(await _geoServiceElasticClient.Client.Indices.ExistsAsync(_indexNames.Places, ct: cancellationToken)).Exists)
        {
            var createIndexResponse = await _geoServiceElasticClient.CreatePlacesIndex(cancellationToken);
            if (createIndexResponse.OriginalException is not null)
                throw createIndexResponse.OriginalException;
            
            _logger.LogElasticIndexCreated(_indexNames.Places);
        }
    }
    

    private async Task<long> GetLastOsmId()
    {
        var lastOsmIdResponse = await _geoServiceElasticClient.Client
            .SearchAsync<Place>(s
                => s.Index(_indexNames.Places)
                    .Fields(f => f.Field(p => p.OsmId))
                    .Sort(sd => sd.Descending(p => p.OsmId))
                    .Size(1));

        return lastOsmIdResponse.Fields.Any()
            ? lastOsmIdResponse.Fields.First().ValueOf<Place, long>(p => p.OsmId)
            : 1;
    }
    
    
    private Task<CountResponse> GetTotalNumberOfPhotonPlaces(CancellationToken cancellationToken)
        => _photonElasticClient.Client.CountAsync<Place>(ct: cancellationToken);
    
    
    private Task<CountResponse> GetTotalNumberOfUploadedPlaces(CancellationToken cancellationToken)
        => _geoServiceElasticClient.Client.CountAsync<Place>(c => c.Index(_indexNames.Places), cancellationToken);
    
    
    private async IAsyncEnumerable<List<Place>> DownloadPhotonPlaces(long lastOsmId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        long? searchAfterId = lastOsmId;
        var batchSize = 10000;
        IReadOnlyCollection<Place> downloadedPlaces;

        int downloadedPlacesCount;
        do
        {
            var photonIndexSearchResponse = await _photonElasticClient.Client.SearchAsync<Place>(s => s
                .MatchAll()
                .Sort(sd => sd.Ascending(p => p.OsmId))
                .SearchAfter(searchAfterId)
                .Size(batchSize), cancellationToken);

            LogDownloadErrors(photonIndexSearchResponse);
            
            downloadedPlaces = photonIndexSearchResponse.Documents;
            
            if (!downloadedPlaces.Any())
                break;
            
            downloadedPlacesCount = downloadedPlaces.Count;
            
            var lastPlace = photonIndexSearchResponse.Documents.LastOrDefault();
            if (lastPlace is not null)
                searchAfterId = lastPlace.OsmId;
            
            yield return downloadedPlaces.ToList();
        } while (downloadedPlacesCount == batchSize);
    }
    
    
    private Task<BulkResponse> UploadPlaces(IEnumerable<Place> items, CancellationToken cancellationToken) 
        => _geoServiceElasticClient.Client.BulkAsync(b 
            => b.Index(_indexNames.Places).IndexMany(items), cancellationToken);


    private void LogDownloadErrors(ISearchResponse<Place> photonIndexSearchResponse)
    {
        if (photonIndexSearchResponse.OriginalException is not null)
            _logger.LogPlacesDownloadErrorOccurred(photonIndexSearchResponse.OriginalException);
    }
    
    
    private void LogUploadErrors(BulkResponse response)
    {
        var stringBuilder = new StringBuilder();
         
        foreach (var item in response.ItemsWithErrors.Where(i => i.Error is not null))
            stringBuilder.AppendLine($"{nameof(item.Id)}: '{item.Id}', {nameof(item.Error)}: '{item.Error}'");

        var logMessage = stringBuilder.ToString();
        if (string.IsNullOrEmpty(logMessage))
            return;
        
        _logger.LogPlacesUploadErrorOccurred(logMessage);
    }
    
    
    private readonly GeoServiceElasticClient _geoServiceElasticClient;
    private readonly PhotonElasticClient _photonElasticClient;
    private readonly IndexNames _indexNames;
    private readonly ILogger<PhotonUpdater> _logger;
}