using HappyTravel.LocationNameNormalizer;
using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Common.Models;

namespace HappyTravel.Wakayama.Api.Services;

public class ReverseGeocodingResponseBuilder
{
    public ReverseGeocodingResponseBuilder(ILocationNameNormalizer locationNameNormalizer)
    {
        _locationNameNormalizer = locationNameNormalizer;
    }


    public ReverseGeocodingResponse Build(Dictionary<string, Place> searchResponse)
    {
        var reverseGeocodingResponse = new ReverseGeocodingResponse();
        foreach (var (index, place) in searchResponse)
        {
            reverseGeocodingResponse.ReverseGeoCodingInfo.Add(int.Parse(index), Build(place));
        }

        return reverseGeocodingResponse;
    }

    
    private ReverseGeoCodingInfo Build(Place place)
    {
        var normalizedCountry = _locationNameNormalizer.GetNormalizedCountryName(place.Country.En ?? place.Country.Default);
        var normalizedCountryCode = _locationNameNormalizer.GetNormalizedCountryCode(normalizedCountry, place.CountryCode);

        var city = GetValue(place.City);
        
        var normalizedCity = !string.IsNullOrEmpty(city)
            ? _locationNameNormalizer.GetNormalizedLocalityName(normalizedCountry, city)
            : string.Empty;
        
        return new()
        {
            Country = normalizedCountry,
            CountryCode = normalizedCountryCode,
            State = GetValue(place.State),
            District = GetValue(place.District),
            County = GetValue(place.County),
            City = normalizedCity,
            Locality = GetValue(place.Locality),
            Street = GetValue(place.Street)
        };
        

        string GetValue(MultiLanguage? obj)
        {
            if (obj is null)
                return string.Empty;
            
            if (obj.En is not null)
                return obj.En;

            return obj.Default ?? string.Empty;
        }
    }


    private readonly ILocationNameNormalizer _locationNameNormalizer;
}