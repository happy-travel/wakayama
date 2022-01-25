using HappyTravel.MultiLanguage;

namespace HappyTravel.Wakayama.Data.Models;

public class LocalityZone
{
    public int Id { get; set; }
    public MultiLanguage<string> Name { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    public int LocalityId { get; set; }
    
    public Locality Locality{ get; set; }
}