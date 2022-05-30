using HappyTravel.LocationNameNormalizer.Extensions;
using HappyTravel.Wakayama.Common.Models;

namespace HappyTravel.Wakayama.Api.Infrastructure.Helpers;

public static class MultiLanguageHelper
{
    public static bool TryGetValue(MultiLanguage? field, out string? value)
    {
        value = null;
        
        if (field is null)
            return false;

        if (!string.IsNullOrWhiteSpace(field.En))
        {
            var enValue =  field.En.ReplaceDiacritics();
            if (!string.IsNullOrWhiteSpace(enValue))
            {
                value = enValue;
                return true;
            }
        }

        if (string.IsNullOrWhiteSpace(field.Default))
            return false;
        
        var defaultValueWithoutDiacritics = field.Default.ReplaceDiacritics();

        value = string.IsNullOrEmpty(defaultValueWithoutDiacritics) 
            ? field.Default 
            : defaultValueWithoutDiacritics;

        return true;
    }
}