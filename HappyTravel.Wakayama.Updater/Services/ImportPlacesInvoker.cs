namespace HappyTravel.Wakayama.Updater.Services;

public class ImportPlacesInvoker : BackgroundService
{
    public ImportPlacesInvoker(IPlacesImporter placesImporter, IHostApplicationLifetime applicationLifetime)
    {
        _placesImporter = placesImporter;
        _applicationLifetime = applicationLifetime;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    { 
        await _placesImporter.Execute(cancellationToken);
        _applicationLifetime.StopApplication();
    }


    private readonly IPlacesImporter _placesImporter;
    private readonly IHostApplicationLifetime _applicationLifetime;
}