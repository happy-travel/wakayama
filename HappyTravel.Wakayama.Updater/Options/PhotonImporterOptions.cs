using Nest;

namespace HappyTravel.Wakayama.Updater.Options;

public class PhotonImporterOptions
{
    public ConnectionSettings ConnectionSettings { get; set; }
    public string Index { get; set; }
    public int Top { get; set; }
}