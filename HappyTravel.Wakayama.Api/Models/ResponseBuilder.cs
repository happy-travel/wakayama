namespace HappyTravel.Wakayama.Api.Models;

public class ResponseBuilder
{
    public SortedDictionary<int, GeoInfoResponse> ReverseGeoCodingInfo { get; set; } = new();
}