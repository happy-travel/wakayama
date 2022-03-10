using Nest;

namespace HappyTravel.Wakayama.Common.Options;

public class ElasticOptions
{
    public ConnectionSettings ClientSettings { get; set; }
    public IndexNames Indexes { get; set; }
}