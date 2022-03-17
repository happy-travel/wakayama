namespace HappyTravel.Wakayama.Api.Models;

public class ReverseGeocodingResponse
{
    public SortedDictionary<int, ReverseGeoCodingInfo> ReverseGeoCodingInfo { get; set; } = new();
}