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
    public ReverseGeocodingService(ElasticGeoServiceClient geoServiceClient, IOptions<ElasticOptions> elasticOptions, ResponseBuilder responseBuilder)
    {
        _geoServiceClient = geoServiceClient;
        _indexes = elasticOptions.Value.Indexes;
        _responseBuilder = responseBuilder;
    }


    public async Task<ReverseGeoCodingResponse> GetCities(ReverseGeocodingRequest request, CancellationToken cancellationToken)
    {
        const int attempts = 10;
        const int distancePerAttempt = 100;
        
        var result = await GetPlacesAtDistance(request, CreateSearchCityRequest, attempts, distancePerAttempt, DistanceUnit.Kilometers, cancellationToken);

        return _responseBuilder.BuildCities(result);
    }


    public async Task<ReverseGeoCodingResponse> Get(ReverseGeocodingRequest request, CancellationToken cancellationToken)
    {
        const int attempts = 10;
        const int distancePerAttempt = 1;
        
        var result = await GetPlacesAtDistance(request, CreateSearchPlaceRequest, attempts, distancePerAttempt, DistanceUnit.Kilometers, cancellationToken);
        
        return _responseBuilder.Build(result);
    }
    
    
    private SearchRequest<Place> CreateSearchPlaceRequest(GeoPoint point, Distance distance, ReverseGeocodingRequest request)
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
                Filter = mustConditions
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

    private SearchRequest<Place> CreateSearchCityRequest(GeoPoint point, Distance distance,
        ReverseGeocodingRequest request)
    {
        var mustConditions = new List<QueryContainer>
        {
            new(new GeoDistanceQuery
            {
                Field = nameof(Place.Coordinate).ToLowerInvariant(),
                Distance = distance,
                Location = new GeoLocation(point.Latitude, point.Longitude)
            }),
            new TermQuery
            {
                Field = nameof(Place.CountryCode).ToLowerInvariant(),
                Value = request.CountryCode
            },
            new TermQuery
            {
                Field = "osm_value",
                Value = "city"
            }
        };
        
        
        return new(_indexes.Places)
        {
            Query = new BoolQuery
            {
                Filter = mustConditions
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

    private async Task<Dictionary<string, Place>> GetPlacesAtDistance(ReverseGeocodingRequest request, Func<GeoPoint, Distance, ReverseGeocodingRequest, ISearchRequest> createSearchRequestFunc,
        int attempts, int distancePerAttempt, DistanceUnit distanceUnit, CancellationToken cancellationToken)
    {
        var elasticClient = _geoServiceClient.Client;
        var coordinates = request.Coordinates;
        var searchResponseStore = new Dictionary<string, Place>(coordinates.Count);
        var notFoundIndexes = Enumerable.Range(0, coordinates.Count).ToList();
        for (var distance = distancePerAttempt; distance <= attempts * distancePerAttempt; distance+= distancePerAttempt)
        {
            var elasticDistance = new Distance(distance, distanceUnit);
            var operations = notFoundIndexes.ToDictionary(i => i.ToString(), i => createSearchRequestFunc(coordinates[i], elasticDistance, request));
            var multiSearchResponse = await elasticClient.MultiSearchAsync(new MultiSearchRequest
            {
                Operations = operations
            }, cancellationToken);
            
            notFoundIndexes = GetNotFoundIndexesAndFillStore(multiSearchResponse, ref searchResponseStore, notFoundIndexes);
            
            if (!notFoundIndexes.Any())
                break;
        }

        return searchResponseStore;
    }


    private List<int> GetNotFoundIndexesAndFillStore(MultiSearchResponse searchResponse, ref Dictionary<string, Place> responseStore, List<int> notFoundIndexes)
    {
        var newNotFoundIndexes = new List<int>();
        
        foreach (var searchIndex in notFoundIndexes)
        {
            var response = searchResponse.GetResponse<Place>(searchIndex.ToString());
            if (response.Documents.Any())
                responseStore[searchIndex.ToString()] = response.Documents.First();
            else
                newNotFoundIndexes.Add(searchIndex);
        }
        
        return newNotFoundIndexes;
    }

    
    private readonly IndexNames _indexes;
    private readonly ElasticGeoServiceClient _geoServiceClient;
    private readonly ResponseBuilder _responseBuilder;
}