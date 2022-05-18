using HappyTravel.LocationNameNormalizer;
using HappyTravel.Wakayama.Api.Infrastructure.Extensions;
using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Common.Models;

namespace HappyTravel.Wakayama.Api.Services;

public class ReverseGeocodingResponseBuilder
{
    public ReverseGeocodingResponseBuilder(ILocationNameNormalizer locationNameNormalizer)
    {
        _locationNameNormalizer = locationNameNormalizer;
    }


    public ResponseBuilder Build(Dictionary<string, Place> response)
    {
        var reverseGeocodingResponse = new ResponseBuilder();
        foreach (var (index, place) in response)
        {
            reverseGeocodingResponse.ReverseGeoCodingInfo.Add(int.Parse(index), Build(place));
        }

        return reverseGeocodingResponse;
    }


    private GeoInfoResponse Build(Place place)
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