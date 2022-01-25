using HappyTravel.MultiLanguage;

namespace HappyTravel.Wakayama.Data.Models;

public class Country
{
    public int Id { get; set; }
    public MultiLanguage<string> Name { get; set; }
    public string CountryCode { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    public int GlobalRegionId { get; set; }
    
    public GlobalRegion GlobalRegion { get; set; }
    public List<ProvinceCountryRelation> ProvinceAndCountryRelations { get; set; }
    public List<LocalityCountryRelation> LocalityAndCountryRelations { get; set; }
}