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
    

    public async Task<ResponseBuilder> Search(ReverseGeocodingRequest request, CancellationToken cancellationToken)
    {
        const int maxSearchDistanceInKm = 10;
        var elasticClient = _geoServiceClient.Client;
        var coordinates = request.Coordinates;
        
        var searchResponseStore = new Dictionary<string, Place>(coordinates.Count);
        var coordinateToSearchIndexes = Enumerable.Range(0, coordinates.Count).ToList();

        for (var attempt = 1; attempt <= maxSearchDistanceInKm; attempt++)
        {
            var operations = new Dictionary<string, ISearchRequest>(coordinateToSearchIndexes.Count);
            var distance = new Distance(attempt, DistanceUnit.Kilometers);

            foreach (var coordinateIndex in coordinateToSearchIndexes)
                operations.Add(coordinateIndex.ToString(), CreateSearchRequest(coordinates[coordinateIndex], distance,  request));

            var multiSearchResponse = await elasticClient.MultiSearchAsync(new MultiSearchRequest
            {
                Operations = operations
            }, cancellationToken);

            var emptyResponseIndexes = new List<int>();
            
            foreach (var coordinateIndex in coordinateToSearchIndexes)
            {
                var response = multiSearchResponse.GetResponse<Place>(coordinateIndex.ToString());
                if (response.Documents.Any())
                    searchResponseStore[coordinateIndex.ToString()] = response.Documents.First();
                else
                    emptyResponseIndexes.Add(coordinateIndex);
            }
            coordinateToSearchIndexes = emptyResponseIndexes;
            
            if (!coordinateToSearchIndexes.Any())
                break;
        }

        return _reverseGeocodingResponseBuilder.Build(searchResponseStore);
    }
    
    
    private SearchRequest<Place> CreateSearchRequest(GeoPoint point, Distance distance, ReverseGeocodingRequest request)
    {
        var mustConditions = new List<QueryContainer>
        {
            new(new GeoDistanceQuery
            {
                Field = nameof(Place.Coordinate).ToLowerInvariant(),
                Distance = distance,
                Location = new GeoLocation(point.Latitude, point.Longitude)
            }),
            new ExistsQuery
            {
                Field = $"{nameof(Place.CountryCode)}".ToLowerInvariant()
            }
        };

        if (string.IsNullOrEmpty(request.CountryCode))
        {
            mustConditions.Add(new ExistsQuery
            {
                Field = $"{nameof(Place.CountryCode)}".ToLowerInvariant()
            });
        }
        else
        {
            mustConditions.Add(new MatchQuery
            {
                Field = $"{nameof(Place.CountryCode)}".ToLowerInvariant(),
                Query = request.CountryCode
            });
        }

        if (request.IsCityRequired)
        {
            mustConditions.Add(new ExistsQuery
            {
                Field = $"{nameof(Place.City)}".ToLowerInvariant()
            });
        }

        return new(_indexes.Places)
        {
            Query = new BoolQuery
            {
                Filter = new[]
                {
                    new QueryContainer(new BoolQuery
                    {
                        Must = mustConditions
                    })
                }
            },
            Sort = new List<ISort>
            {
                new GeoDistanceSort
                {
                    Field = nameof(Place.Coordinate).ToLowerInvariant(),
                    Order = SortOrder.Ascending,
                    Points = new[]
                    {
                        new GeoCoordinate(point.Latitude, point.Longitude)
                    }
                }
            },
            From = 0,
            Size = 1
        };
    }


    private readonly IndexNames _indexes;
    private readonly ElasticGeoServiceClient _geoServiceClient;
    private readonly ReverseGeocodingResponseBuilder _reverseGeocodingResponseBuilder;
}