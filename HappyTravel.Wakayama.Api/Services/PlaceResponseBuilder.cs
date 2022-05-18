using HappyTravel.LocationNameNormalizer;
using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Common.Models;
using HappyTravel.Wakayama.Api.Infrastructure.Extensions;

namespace HappyTravel.Wakayama.Api.Services;

public class PlaceResponseBuilder
{
    public PlaceResponseBuilder(ILocationNameNormalizer locationNameNormalizer)
    {
        _locationNameNormalizer = locationNameNormalizer;
    }
    
    
    public GeoInfoResponse BuildCities(Place geoInfo)
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
    
    
    public GeoInfoResponse BuildCountries(Place geoInfo)
    {
        var country = _locationNameNormalizer.GetNormalizedCountryName(geoInfo.Country.GetValue());
        
        return new()
        {
            OsmId = geoInfo.OsmId,
            CountryCode = geoInfo.CountryCode,
            Country = country
        };
    }
    

    private readonly ILocationNameNormalizer _locationNameNormalizer;
}