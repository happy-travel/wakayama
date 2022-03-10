using Nest;

namespace HappyTravel.Wakayama.Updater.Options;

public class PhotonUpdaterOptions
{
    public ConnectionSettings ConnectionSettings { get; set; }
    public string Index { get; set; }
}