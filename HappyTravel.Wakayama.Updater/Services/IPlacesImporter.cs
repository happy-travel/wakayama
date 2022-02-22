namespace HappyTravel.Wakayama.Updater.Services;

public interface IPlacesImporter
{
    Task Execute(CancellationToken cancellationToken);
}