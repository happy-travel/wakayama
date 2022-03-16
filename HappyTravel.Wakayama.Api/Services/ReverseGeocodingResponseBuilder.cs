using HappyTravel.LocationNameNormalizer;
using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Common.Models;
using Nest;

namespace HappyTravel.Wakayama.Api.Services;

public class ReverseGeocodingResponseBuilder
{
    public ReverseGeocodingResponseBuilder(ILocationNameNormalizer locationNameNormalizer)
    {
        _locationNameNormalizer = locationNameNormalizer;
    }
    
    
    public ReverseGeocodingResponse Build(MultiSearchResponse searchResponse)
    {
        var responses = searchResponse.GetResponses<Place>().ToList();
        var reverseGeocodingResponse = new ReverseGeocodingResponse();
        for (var i = 0; i < responses.Count; i++)
        {
            var response = responses[i];
            if (response.Documents.Any())
            {
                reverseGeocodingResponse.ReverseGeoCodingInfo.Add(i, Build(response.Documents.First()));
            }
        }

        searchResponse.GetResponses<Place>();

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