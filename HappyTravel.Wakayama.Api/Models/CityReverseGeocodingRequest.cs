using HappyTravel.Geography;

namespace HappyTravel.Wakayama.Api.Models;

public record CityReverseGeoCodingRequest
{
    public string CountryCode {get; init;} = string.Empty;
    public List<GeoPoint> Coordinates { get; init; } = new();
}