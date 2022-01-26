namespace HappyTravel.Wakayama.Data.Models;

public class LocalityProvinceRelation
{
    public int Id { get; set; }
    public List<string> RelatedLanguageCodes { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    public int LocalityId { get; set; }
    public int ProvinceId { get; set; }
    
    public Locality Locality { get; set; }
    public Province Province { get; set; }
}