using HappyTravel.Wakayama.Api.Models;

namespace HappyTravel.Wakayama.Api.Services;

public interface IPlaceSearchingService
{
    Task<List<GeoInfoResponse>> GetCities(string countryCode, CancellationToken cancellationToken);
    Task<List<GeoInfoResponse>> GetCountries(CancellationToken cancellationToken);
}