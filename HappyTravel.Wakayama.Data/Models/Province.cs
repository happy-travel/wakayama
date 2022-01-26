using HappyTravel.MultiLanguage;

namespace HappyTravel.Wakayama.Data.Models;

public class Province
{
    public int Id { get; set; }
    public MultiLanguage<string> Name { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    
    public List<LocalityProvinceRelation> LocalityAndProvinceRelations { get; set; }
    public List<ProvinceCountryRelation> ProvinceAndCountryRelations {get; set; }
}