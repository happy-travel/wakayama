using System;
using Microsoft.Extensions.Logging;

namespace HappyTravel.Wakayama.Common.Logging;

public static partial class LoggerExtensions
{
    [LoggerMessage(82101, LogLevel.Information, "Update is started. Launch mode is '{launchMode}'. Start osm_id is '{osmId}'")]
    static partial void UpdateStarted(ILogger logger, string launchMode, string osmId);
    
    [LoggerMessage(82102, LogLevel.Information, "{numberOfUploadedPlaces} out of {totalNumberOfPlaces} items have been uploaded to '{index}' index")]
    static partial void PlacesUploaded(ILogger logger, long numberOfUploadedPlaces, long totalNumberOfPlaces, string index);
    
    [LoggerMessage(82103, LogLevel.Information, "Update is completed")]
    static partial void UpdateCompleted(ILogger logger);
    
    [LoggerMessage(82104, LogLevel.Information, "Index '{indexName}' has been created")]
    static partial void ElasticIndexCreated(ILogger logger, string indexName);
    
    [LoggerMessage(82105, LogLevel.Information, "Index '{indexName}' has been removed")]
    static partial void ElasticIndexRemoved(ILogger logger, string indexName);
    
    [LoggerMessage(82150, LogLevel.Error, "An update error has been occured")]
    static partial void UpdateErrorOccurred(ILogger logger, System.Exception exception);
    
    [LoggerMessage(82151, LogLevel.Error, "A download error has been occured")]
    static partial void PlacesDownloadErrorOccurred(ILogger logger, System.Exception exception);
    
    [LoggerMessage(82152, LogLevel.Error, "Update error: {error}")]
    static partial void PlacesUploadErrorOccurred(ILogger logger, string error);
    
    
    
    public static void LogUpdateStarted(this ILogger logger, string launchMode, string osmId)
        => UpdateStarted(logger, launchMode, osmId);
    
    public static void LogPlacesUploaded(this ILogger logger, long numberOfUploadedPlaces, long totalNumberOfPlaces, string index)
        => PlacesUploaded(logger, numberOfUploadedPlaces, totalNumberOfPlaces, index);
    
    public static void LogUpdateCompleted(this ILogger logger)
        => UpdateCompleted(logger);
    
    public static void LogElasticIndexCreated(this ILogger logger, string indexName)
        => ElasticIndexCreated(logger, indexName);
    
    public static void LogElasticIndexRemoved(this ILogger logger, string indexName)
        => ElasticIndexRemoved(logger, indexName);
    
    public static void LogUpdateErrorOccurred(this ILogger logger, System.Exception exception)
        => UpdateErrorOccurred(logger, exception);
    
    public static void LogPlacesDownloadErrorOccurred(this ILogger logger, System.Exception exception)
        => PlacesDownloadErrorOccurred(logger, exception);
    
    public static void LogPlacesUploadErrorOccurred(this ILogger logger, string error)
        => PlacesUploadErrorOccurred(logger, error);
}