namespace HappyTravel.Wakayama.Api.Models;

public class ReverseGeoCodingResponse
{
    public SortedDictionary<int, GeoInfoResponse> ReverseGeoCodingInfo { get; set; } = new();
}