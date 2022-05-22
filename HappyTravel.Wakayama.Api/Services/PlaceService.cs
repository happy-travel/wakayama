using HappyTravel.Wakayama.Api.Models;
using HappyTravel.Wakayama.Common.ElasticClients;
using HappyTravel.Wakayama.Common.Models;
using HappyTravel.Wakayama.Common.Options;
using Microsoft.Extensions.Options;
using Nest;

namespace HappyTravel.Wakayama.Api.Services;

public class PlaceService : IPlaceService
{
    public PlaceService(ElasticGeoServiceClient elasticGeoServiceClient, ResponseBuilder responseBuilder, IOptions<ElasticOptions> elasticOptions)
    {
        _elasticGeoServiceClient = elasticGeoServiceClient;
        _elasticOptions = elasticOptions.Value;
        _responseBuilder = responseBuilder;
    }

    
    public async Task<List<GeoInfoResponse>> GetCities(string countryCode, CancellationToken cancellationToken)
    {
        var searchDescriptor = GetCitySearchDescriptor(countryCode.ToUpperInvariant());
        var places = await GetPlaces(searchDescriptor, cancellationToken);

        return places.Select(_responseBuilder.BuildCity).ToList();
    }
    
    
    public async Task<List<GeoInfoResponse>> GetCountries(CancellationToken cancellationToken)
    {
        var searchDescriptor = GetCountrySearchDescriptor();
        var places = await GetPlaces(searchDescriptor, cancellationToken);
        
        return places.Select(_responseBuilder.BuildCountry).ToList();
    }


    private SearchDescriptor<Place> GetCitySearchDescriptor(string countryCode)
        => new SearchDescriptor<Place>(_elasticOptions.Indexes.Places).Query(qc
                => qc.Bool(bq => bq.Filter(qcd
                    => qcd.Term(tq => tq.Field(p => p.CountryCode).Value(countryCode)), qcd 
                    => qcd.Exists(eq => eq.Field(p => p.Name)), qcd
                    => qcd.Term(tq => tq.Field(p => p.OsmValue).Value("city")))))
            .Sort(sd => sd.Descending(p => p.OsmId));


    private SearchDescriptor<Place> GetCountrySearchDescriptor()
        => new SearchDescriptor<Place>(_elasticOptions.Indexes.Places).Query(qc 
            => qc.Bool(bq => bq.Filter(qcd 
                    => qcd.Term(tq => tq.Field(p => p.OsmValue).Value("country")), qcd 
                    => qcd.Exists(eq => eq.Field(p => p.Name)), qcd 
                    => qcd.Exists(eq => eq.Field(p => p.CountryCode)))))
            .Sort(sd => sd.Ascending(p => p.CountryCode));

    
    private async Task<List<Place>> GetPlaces(SearchDescriptor<Place> searchDescriptor, CancellationToken cancellationToken = default)
    {
        var cities = new List<Place>();
        long? searchAfterId = null;
        const int batchSize = 10000;
        searchDescriptor.Size(batchSize);
        ISearchResponse<Place> searchResponse;
        do
        {
            if (searchAfterId is not null)
                searchDescriptor.SearchAfter(searchAfterId);
            
            searchResponse = await _elasticGeoServiceClient.Client.SearchAsync<Place>(
                searchDescriptor, cancellationToken);

            if (!searchResponse.Documents.Any())
                break;
            
            searchAfterId = GetSearchAfterId(searchResponse);

            cities.AddRange(searchResponse.Documents);
            
        } while (searchAfterId is not null && searchResponse.Documents.Count == batchSize);

        return cities;
    }
    
    
    private long? GetSearchAfterId(ISearchResponse<Place> searchResponse)
        => searchResponse.Documents.LastOrDefault()?.OsmId;


    private readonly ElasticGeoServiceClient _elasticGeoServiceClient;
    private readonly ResponseBuilder _responseBuilder;
    private readonly ElasticOptions _elasticOptions;
}