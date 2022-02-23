using HappyTravel.Wakayama.Updater.Options;

namespace HappyTravel.Wakayama.Updater.Services;

public interface IPlacesUpdater
{
    Task Execute(LaunchMode launchMode, CancellationToken cancellationToken);
}