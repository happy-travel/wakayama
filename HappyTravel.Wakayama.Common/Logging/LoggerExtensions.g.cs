using System;
using Microsoft.Extensions.Logging;

namespace HappyTravel.Wakayama.Common.Logging;

public static partial class LoggerExtensions
{
    [LoggerMessage(82101, LogLevel.Information, "The import execution is started")]
    static partial void ImportStarted(ILogger logger);
    
    [LoggerMessage(82102, LogLevel.Information, "{uploadedPlacesNumber} out of {totalPlacesNumber} items have been uploaded to the index")]
    static partial void PlacesUploaded(ILogger logger, long uploadedPlacesNumber, long totalPlacesNumber);
    
    [LoggerMessage(82103, LogLevel.Information, "The import execution is completed")]
    static partial void ImportCompleted(ILogger logger);
    
    [LoggerMessage(82104, LogLevel.Information, "Index '{indexName}' has been created")]
    static partial void ElasticIndexCreated(ILogger logger, string indexName);
    
    [LoggerMessage(82105, LogLevel.Information, "Index '{indexName}' has been removed")]
    static partial void ElasticIndexRemoved(ILogger logger, string indexName);
    
    [LoggerMessage(82150, LogLevel.Error, "An import error has been occured")]
    static partial void ImportErrorOccurred(ILogger logger, System.Exception exception);
    
    [LoggerMessage(82151, LogLevel.Error, "A download error has been occured")]
    static partial void PlacesDownloadErrorOccurred(ILogger logger, System.Exception exception);
    
    [LoggerMessage(82152, LogLevel.Error, "The upload error: {error}")]
    static partial void PlacesUploadErrorOccurred(ILogger logger, string error);
    
    
    
    public static void LogImportStarted(this ILogger logger)
        => ImportStarted(logger);
    
    public static void LogPlacesUploaded(this ILogger logger, long uploadedPlacesNumber, long totalPlacesNumber)
        => PlacesUploaded(logger, uploadedPlacesNumber, totalPlacesNumber);
    
    public static void LogImportCompleted(this ILogger logger)
        => ImportCompleted(logger);
    
    public static void LogElasticIndexCreated(this ILogger logger, string indexName)
        => ElasticIndexCreated(logger, indexName);
    
    public static void LogElasticIndexRemoved(this ILogger logger, string indexName)
        => ElasticIndexRemoved(logger, indexName);
    
    public static void LogImportErrorOccurred(this ILogger logger, System.Exception exception)
        => ImportErrorOccurred(logger, exception);
    
    public static void LogPlacesDownloadErrorOccurred(this ILogger logger, System.Exception exception)
        => PlacesDownloadErrorOccurred(logger, exception);
    
    public static void LogPlacesUploadErrorOccurred(this ILogger logger, string error)
        => PlacesUploadErrorOccurred(logger, error);
}