using System.Text.Json.Serialization;
using Nest;

namespace HappyTravel.Wakayama.Common.Models;

public record MultiLanguageName : MultiLanguage
{
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