using System.Net;
using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Wakayama.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{v:apiVersion}/geocoding")]
public class ReverseGeocodingController : BaseController
{
    public ReverseGeocodingController(IReverseGeocodingService reverseGeocodingService)
    {
        _reverseGeocodingService = reverseGeocodingService;
    }

    
    /// <summary>
    /// Returns cities by coordinates
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("reverse/cities")]
    [ProducesResponseType(typeof(SortedDictionary<int, GeoInfoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReverseCities([FromBody] CityReverseGeoCodingRequest request, CancellationToken cancellationToken) 
        => Ok(await _reverseGeocodingService.GetCities(request, cancellationToken));
    
    
    /// <summary>
    /// Returns geo-information by coordinates
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("reverse")]
    [ProducesResponseType(typeof(SortedDictionary<int, GeoInfoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReverse([FromBody] ReverseGeoCodingRequest request, CancellationToken cancellationToken) 
        => Ok(await _reverseGeocodingService.Get(request, cancellationToken));


    private readonly IReverseGeocodingService _reverseGeocodingService;
}