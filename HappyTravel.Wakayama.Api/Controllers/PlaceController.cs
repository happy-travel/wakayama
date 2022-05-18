using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Wakayama.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{v:apiVersion}/places")]
public class PlaceController : BaseController
{
    public PlaceController(IPlaceSearchingService placeSearchingService)
    {
        _placeSearchingService = placeSearchingService;
    }

    
    /// <summary>
    /// Returns the list of cities
    /// </summary>
    /// <param name="countryCode">Two-digit country code</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("countries/{countryCode}/cities")]
    [ProducesResponseType(typeof(List<GeoInfoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCities(string countryCode, CancellationToken cancellationToken = default)
        => Ok(await _placeSearchingService.GetCities(countryCode, cancellationToken));
    
    
    /// <summary>
    /// Returns the list of countries
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("countries")]
    [ProducesResponseType(typeof(List<GeoInfoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCountries(CancellationToken cancellationToken = default)
        => Ok(await _placeSearchingService.GetCountries(cancellationToken));
    
    
    private readonly IPlaceSearchingService _placeSearchingService;
}