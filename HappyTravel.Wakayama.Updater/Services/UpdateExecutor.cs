using HappyTravel.Wakayama.Updater.Options;
using Microsoft.Extensions.Options;

namespace HappyTravel.Wakayama.Updater.Services;

public class UpdateExecutor : BackgroundService
{
    public UpdateExecutor(IPlacesUpdater placesUpdater, IHostApplicationLifetime applicationLifetime, IOptions<UpdaterOptions> updaterOptions)
    {
        _placesUpdater = placesUpdater;
        _updaterOptions = updaterOptions.Value;
        _applicationLifetime = applicationLifetime;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    { 
        await _placesUpdater.Execute(_updaterOptions.LaunchMode, cancellationToken);
        
        _applicationLifetime.StopApplication();
    }


    private readonly UpdaterOptions _updaterOptions;
    private readonly IPlacesUpdater _placesUpdater;
    private readonly IHostApplicationLifetime _applicationLifetime;
}