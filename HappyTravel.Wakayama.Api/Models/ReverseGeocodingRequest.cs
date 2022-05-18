using HappyTravel.Geography;

namespace HappyTravel.Wakayama.Api.Models;

public class ReverseGeocodingRequest
{
    public List<GeoPoint> Coordinates { get; set; }
    public bool IsCityRequired { get; set; }
    public string CountryCode { get; set; }
}