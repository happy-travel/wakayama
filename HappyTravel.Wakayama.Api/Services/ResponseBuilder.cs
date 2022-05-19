using HappyTravel.LocationNameNormalizer;
using HappyTravel.Wakayama.Api.Infrastructure.Extensions;
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
        var country = _locationNameNormalizer.GetNormalizedCountryName(geoInfo.Country.GetValue());
        var city = _locationNameNormalizer.GetNormalizedLocalityName(country, geoInfo.Name.GetValue());
        
        return new()
        {
            OsmId = geoInfo.OsmId,
            CountryCode = geoInfo.CountryCode,
            Country = country,
            State = geoInfo.State.GetValue(),
            County = geoInfo.County.GetValue(),
            City = city,
            District = geoInfo.District.GetValue(),
            Locality = geoInfo.Locality.GetValue()
        };
    }


    public GeoInfoResponse BuildCountry(Place geoInfo)
    {
        var country = _locationNameNormalizer.GetNormalizedCountryName(geoInfo.Country.GetValue());

        return new()
        {
            OsmId = geoInfo.OsmId,
            CountryCode = geoInfo.CountryCode,
            Country = country
        };
    }

    
    private GeoInfoResponse BuildCommon(Place place)
    {
        var normalizedCountry = _locationNameNormalizer.GetNormalizedCountryName(place.Country!.En ?? place.Country.Default);
        var normalizedCountryCode = _locationNameNormalizer.GetNormalizedCountryCode(normalizedCountry, place.CountryCode);

        var city = place.City.GetValue();

        var normalizedCity = !string.IsNullOrEmpty(city)
            ? _locationNameNormalizer.GetNormalizedLocalityName(normalizedCountry, city)
            : null;

        return new()
        {
            OsmId = place.OsmId,
            Country = normalizedCountry,
            CountryCode = normalizedCountryCode,
            State = place.State.GetValue(),
            District = place.State.GetValue(),
            County = place.State.GetValue(),
            City = normalizedCity,
            Locality = place.State.GetValue(),
            Street = place.State.GetValue()
        };
    }


    private readonly ILocationNameNormalizer _locationNameNormalizer;
}