using HappyTravel.MultiLanguage;

namespace HappyTravel.Wakayama.Data.Models;

public class GlobalRegion
{
    public int Id { get; set; }
    public MultiLanguage<string> Name { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    
    public List<Country> Countries { get; set; } 
}