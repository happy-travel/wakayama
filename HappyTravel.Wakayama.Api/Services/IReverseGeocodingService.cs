using HappyTravel.Wakayama.Api.Models;

namespace HappyTravel.Wakayama.Api.Services;

public interface IReverseGeocodingService
{ 
    Task<ReverseGeoCodingResponse> Get(ReverseGeoCodingRequest request, CancellationToken cancellationToken);
    Task<ReverseGeoCodingResponse> GetCities(CityReverseGeoCodingRequest request, CancellationToken cancellationToken);
}