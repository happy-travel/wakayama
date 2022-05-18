using HappyTravel.Wakayama.Api.Models;

namespace HappyTravel.Wakayama.Api.Services;

public interface IReverseGeocodingService
{ 
    Task<ResponseBuilder> Search(ReverseGeocodingRequest request, CancellationToken cancellationToken);
}