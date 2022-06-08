using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Wakayama.Api.Controllers;

public class BaseController : ControllerBase 
{
    protected string LanguageCode => CultureInfo.CurrentCulture.Name;
        
    protected IActionResult BadRequestWithProblemDetails(string details)
        => BadRequest(new ProblemDetails
        {
            Detail = details,
            Status = (int) HttpStatusCode.BadRequest
        });
}