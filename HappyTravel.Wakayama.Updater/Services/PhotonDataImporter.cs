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

public class PhotonDataImporter : IPlacesImporter
{
    public PhotonDataImporter(GeoServiceElasticClient geoServiceElasticClient, PhotonElasticClient photonElasticClient, IOptions<ElasticOptions> elasticOptions, IOptions<PhotonImporterOptions> importerOptions, ILogger<PhotonDataImporter> logger)
    {
        _geoServiceElasticClient = geoServiceElasticClient.Client;
        _photonElasticClient = photonElasticClient.Client;
        _indexNames = elasticOptions.Value.Indexes;
        _importerOptions = importerOptions.Value;
        _logger = logger;
    }

    
    public async Task Execute(CancellationToken cancellationToken)
    {
        _logger.LogImportStarted();
        try
        {
            var totalPlaces = (await GetCountOfPlaces(cancellationToken)).Count;
            var numberOfSuccessfullyUploadedItems = 0L;
            
            await foreach (var photonPlaces in GetPhotonPlaces(cancellationToken))
            {
                var uploadResponse = await AddPlaces(photonPlaces, cancellationToken);
                
                LogUploadErrors(uploadResponse);
                
                numberOfSuccessfullyUploadedItems += photonPlaces.Count - uploadResponse.ItemsWithErrors.Count(i => i.Error is not null);
                _logger.LogPlacesUploaded(numberOfSuccessfullyUploadedItems, totalPlaces);
            }
        }
        catch (Exception ex)
        {
            _logger.LogImportErrorOccurred(ex);
        }

        _logger.LogImportCompleted();
    }


    private Task<CountResponse> GetCountOfPlaces(CancellationToken cancellationToken)
        => _photonElasticClient.CountAsync<Place>(ct: cancellationToken);
    
    
    private async IAsyncEnumerable<List<Place>> GetPhotonPlaces([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        long? firstOsmId = 1;
        var batchSize = _importerOptions.Top;
        IReadOnlyCollection<Place> downloadedPlaces;

        do
        {
            var photonIndexSearchResponse = await _photonElasticClient.SearchAsync<Place>(s => s
                .MatchAll()
                .Sort(sd => sd.Ascending(p => p.OsmId))
                .SearchAfter(firstOsmId)
                .Size(batchSize), cancellationToken);

            LogDownloadErrors(photonIndexSearchResponse);
            
            downloadedPlaces = photonIndexSearchResponse.Documents;
            
            var lastPlace = photonIndexSearchResponse.Documents.LastOrDefault();
            if (lastPlace is not null)
                firstOsmId = lastPlace.OsmId;
            
            yield return downloadedPlaces.ToList();
        } while (downloadedPlaces.Count == batchSize);
    }


    private Task<BulkResponse> AddPlaces(IEnumerable<Place> items, CancellationToken cancellationToken) 
        => _geoServiceElasticClient.BulkAsync(b 
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
    
    
    private readonly IElasticClient _geoServiceElasticClient;
    private readonly IElasticClient _photonElasticClient;
    private readonly IndexNames _indexNames;
    private readonly ILogger<PhotonDataImporter> _logger;
    private readonly PhotonImporterOptions _importerOptions;
}