using HappyTravel.LocationNameNormalizer;
using HappyTravel.Wakayama.Api.Infrastructure.Extensions;
using HappyTravel.Wakayama.Api.Infrastructure.Helpers;
using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Common.Models;

namespace HappyTravel.Wakayama.Api.Services;

public class ResponseBuilder
{
    public ResponseBuilder(ILocationNameNormalizer locationNameNormalizer)
    {
        _locationNameNormalizer = locationNameNormalizer;
    }


    public ReverseGeoCodingResponse Build(Dictionary<string, Place> response)
    {
        var reverseGeocodingResponse = new ReverseGeoCodingResponse();
        foreach (var (index, place) in response)
        {
            reverseGeocodingResponse.ReverseGeoCodingInfo.Add(int.Parse(index), BuildCommon(place));
        }

        return reverseGeocodingResponse;
    }
    
    
    public ReverseGeoCodingResponse BuildCities(Dictionary<string, Place> response)
    {
        var reverseGeocodingResponse = new ReverseGeoCodingResponse();
        foreach (var (index, place) in response)
        {
            reverseGeocodingResponse.ReverseGeoCodingInfo.Add(int.Parse(index), BuildCity(place));
        }

        return reverseGeocodingResponse;
    }
    
    
    public GeoInfoResponse BuildCity(Place geoInfo)
    {
        MultiLanguageHelper.TryGetValue(geoInfo.Country, out var country);
        MultiLanguageHelper.TryGetValue(geoInfo.Name, out var city);
        MultiLanguageHelper.TryGetValue(geoInfo.State, out var state);
        MultiLanguageHelper.TryGetValue(geoInfo.County, out var county);
        MultiLanguageHelper.TryGetValue(geoInfo.District, out var district);
        MultiLanguageHelper.TryGetValue(geoInfo.Locality, out var locality);
        
        var normalizedCity = _locationNameNormalizer.GetNormalizedLocalityName(country, city);

        return new()
        {
            OsmId = geoInfo.OsmId,
            CountryCode = geoInfo.CountryCode,
            Country = country,
            State = state,
            County = county,
            City = normalizedCity,
            District = district,
            Locality = locality
        };
    }


    public GeoInfoResponse BuildCountry(Place geoInfo)
    {
        MultiLanguageHelper.TryGetValue(geoInfo.Country, out var country);
        var normalizedCountryName = _locationNameNormalizer.GetNormalizedCountryName(country);

        return new()
        {
            OsmId = geoInfo.OsmId,
            CountryCode = geoInfo.CountryCode,
            Country = normalizedCountryName
        };
    }

    
    private GeoInfoResponse BuildCommon(Place place)
    {
        MultiLanguageHelper.TryGetValue(place.Country, out var country);
        var normalizedCountry = _locationNameNormalizer.GetNormalizedCountryName(place.Country!.En ?? place.Country.Default);
        var normalizedCountryCode = _locationNameNormalizer.GetNormalizedCountryCode(normalizedCountry, place.CountryCode);
        MultiLanguageHelper.TryGetValue(place.City, out var city);
        
        var normalizedCity = !string.IsNullOrEmpty(city)
            ? _locationNameNormalizer.GetNormalizedLocalityName(normalizedCountry, city)
            : null;
        
        MultiLanguageHelper.TryGetValue(place.State, out var state);
        MultiLanguageHelper.TryGetValue(place.County, out var county);
        MultiLanguageHelper.TryGetValue(place.District, out var district);
        MultiLanguageHelper.TryGetValue(place.Locality, out var locality);
        MultiLanguageHelper.TryGetValue(place.Street, out var street);
        
        return new()
        {
            OsmId = place.OsmId,
            Country = normalizedCountry,
            CountryCode = normalizedCountryCode,
            State = state,
            District = district,
            County = county,
            City = normalizedCity,
            Locality = locality,
            Street = street
        };
    }


    private readonly ILocationNameNormalizer _locationNameNormalizer;
}