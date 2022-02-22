using System.Text.Json.Serialization;
using Nest;

namespace HappyTravel.Wakayama.Common.Models;

public record MultiLanguageName
{
    [PropertyName("default")]
    public string Default { get; set; }
    
    [PropertyName("en")]
    public string En { get; set; }
    
    [PropertyName("de")]
    public string De { get; set; }
    
    [PropertyName("fr")]
    public string Fr { get; set; }
    
    [PropertyName("alt")]
    public string Alt { get; set; }
    
    [PropertyName("housename")]
    public string HouseName { get; set; }
    
    [PropertyName("int")]
    public string Int { get; set; }
    
    [PropertyName("loc")]
    public string Loc { get; set; }
    
    [PropertyName("old")]
    public string Old { get; set; }
    
    [PropertyName("reg")]
    public string Reg { get; set; }
}