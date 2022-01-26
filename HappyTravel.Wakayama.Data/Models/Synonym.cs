using HappyTravel.MultiLanguage;

namespace HappyTravel.Wakayama.Data.Models;

public class Synonym
{
    public int Id { get; set; }
    public MultiLanguage<string> Name { get; set; }
    public int SourceId { get; set; }
    public SynonymTypes SynonymType { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
}