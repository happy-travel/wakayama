namespace HappyTravel.Wakayama.Data.Models;

public class ProvinceCountryRelation
{
    public int Id { get; set; }
    public List<string> RelatedLanguageCodes { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    public int ProvinceId { get; set; }
    public int CountryId { get; set; }
    
    public Province Province { get; set; }
    public Country Country { get; set; }
}