using HappyTravel.MultiLanguage;

namespace HappyTravel.Wakayama.Data.Models;

public class Locality
{
    public int Id { get; set; }
    public MultiLanguage<string> Name { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    
    
    public List<LocalityProvinceRelation> LocalityAndProvinceRelations { get; set; }
    public List<LocalityCountryRelation> LocalityAndCountryRelations { get; set; }
    public List<LocalityZone> LocalityZones { get; set; }
}