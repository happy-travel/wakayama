using HappyTravel.LocationNameNormalizer.Extensions;
using HappyTravel.Wakayama.Common.Models;

namespace HappyTravel.Wakayama.Api.Infrastructure.Extensions;

public static class MultiLanguageExtensions
{
    public static string? GetValue(this MultiLanguage? value)
    {
        if (value is null)
            return null;

        if (!string.IsNullOrWhiteSpace(value.En))
        { 
            var enValue =  value.En.ReplaceDiacritics();
            if (!string.IsNullOrWhiteSpace(enValue))
                return enValue;
        }

        if (string.IsNullOrWhiteSpace(value.Default)) 
            return null;
        
        var defaultValueWithoutDiacritics = value.Default.ReplaceDiacritics();
            
        return string.IsNullOrEmpty(defaultValueWithoutDiacritics) 
            ? value.Default 
            : defaultValueWithoutDiacritics;
    }
}