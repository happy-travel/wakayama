using HappyTravel.Geography;
using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Common.ElasticClients;
using HappyTravel.Wakayama.Common.Models;
using HappyTravel.Wakayama.Common.Options;
using Microsoft.Extensions.Options;
using Nest;

namespace HappyTravel.Wakayama.Api.Services;

public class ReverseGeocodingService : IReverseGeocodingService
{
    public ReverseGeocodingService(ElasticGeoServiceClient geoServiceClient, IOptions<ElasticOptions> elasticOptions, ReverseGeocodingResponseBuilder reverseGeocodingResponseBuilder)
    {
        _geoServiceClient = geoServiceClient;
        _indexes = elasticOptions.Value.Indexes;
        _reverseGeocodingResponseBuilder = reverseGeocodingResponseBuilder;
    }


    public async Task<ReverseGeocodingResponse> Search(ReverseGeocodingRequest request, CancellationToken cancellationToken)
    {
        var elasticClient = _geoServiceClient.Client;
        var distance = new Distance(1, DistanceUnit.Kilometers);
        var operations = new Dictionary<string, ISearchRequest>();
        
        for (var i = 0; i < request.Coordinates.Count; i++)
        {
            var point = request.Coordinates[i];
            var searchRequest = CreateSearchRequest(point, distance);
            operations.Add(i.ToString(), searchRequest);
        }
        
        var multiSearchResponse = await elasticClient.MultiSearchAsync(new MultiSearchRequest
        {
            Operations = operations
        }, cancellationToken);

        return _reverseGeocodingResponseBuilder.Build(multiSearchResponse);
    }


    private SearchRequest<Place> CreateSearchRequest(GeoPoint point, Distance distance)
        => new (_indexes.Places)
    {
        Query = new GeoDistanceQuery
        {
            Field = nameof(Place.Coordinate).ToLowerInvariant(),
            Distance = distance,
            Location = new GeoLocation(point.Latitude, point.Longitude)
        },
        Sort = new List<ISort>
        {
            new GeoDistanceSort
            {
                Field = nameof(Place.Coordinate).ToLowerInvariant(),
                Order = SortOrder.Ascending,
                Points = new []
                {
                    new GeoCoordinate(point.Latitude, point.Longitude)
                }
            }
        },
        From = 0,
        Size = 1
    };

    
    private readonly IndexNames _indexes;
    private readonly ElasticGeoServiceClient _geoServiceClient;
    private readonly ReverseGeocodingResponseBuilder _reverseGeocodingResponseBuilder;
}