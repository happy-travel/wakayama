using FluentValidation;
using HappyTravel.Wakayama.Api.Models;
using NetTopologySuite.Geometries;

namespace HappyTravel.Wakayama.Api.Validators;

public class ReverseGeocodingRequestValidator : AbstractValidator<ReverseGeocodingRequest>
{
    public ReverseGeocodingRequestValidator()
    {
        const int maxNumberOfCoordinates = 1000;
        
        RuleFor(r => r.Coordinates).NotEmpty().Must(r => r.Count <= maxNumberOfCoordinates).WithMessage($"The number of {nameof(Coordinates)} must be less than {maxNumberOfCoordinates}");
    }
}