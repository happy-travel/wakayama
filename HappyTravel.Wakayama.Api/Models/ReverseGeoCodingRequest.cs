using HappyTravel.Geography;

namespace HappyTravel.Wakayama.Api.Models;

public record ReverseGeoCodingRequest
{
    public List<GeoPoint> Coordinates { get; init; } = new();
    public bool IsCityRequired { get; init; } = false;
    public string? CountryCode { get; init; }
}