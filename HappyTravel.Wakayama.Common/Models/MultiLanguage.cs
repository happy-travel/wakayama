using Nest;

namespace HappyTravel.Wakayama.Common.Models;

public record MultiLanguage
{
    [PropertyName("default")]
    public string? Default { get; set; }
    
    [PropertyName("en")]
    public string? En { get; set; }
    
    [PropertyName("de")]
    public string? De { get; set; }
    
    [PropertyName("fr")]
    public string? Fr { get; set; }
}