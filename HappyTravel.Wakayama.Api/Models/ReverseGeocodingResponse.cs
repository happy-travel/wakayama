namespace HappyTravel.Wakayama.Api.Models;

public class ReverseGeocodingResponse
{
    public Dictionary<int, ReverseGeoCodingInfo> ReverseGeoCodingInfo { get; set; } = new();
}