using HappyTravel.Geography;

namespace HappyTravel.Wakayama.Api.Models;

public class ReverseGeocodingRequest
{
    public List<GeoPoint> Coordinates { get; set; }
}