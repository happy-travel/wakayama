using HappyTravel.Wakayama.Common.Helpers;
using HappyTravel.Wakayama.Common.Logging;
using HappyTravel.Wakayama.Common.Models;
using HappyTravel.Wakayama.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

namespace HappyTravel.Wakayama.Common.ElasticClients;

public sealed class GeoServiceElasticClient
{
    public GeoServiceElasticClient(IOptions<ElasticOptions> elasticOptions, ILogger<GeoServiceElasticClient> logger)
    {
        _logger = logger;
        Client = new ElasticClient(elasticOptions.Value.ClientSettings);
        _indexNames = elasticOptions.Value.Indexes;

        if (Client.Indices.Exists(_indexNames.Places).Exists)
        {
            var removeIndexResponse = RemoveIndex(_indexNames.Places);
            if (removeIndexResponse.OriginalException is not null)
                throw removeIndexResponse.OriginalException;

            _logger.LogElasticIndexRemoved(_indexNames.Places);
        }

        var response = CreatePlacesIndex();
        if (response.OriginalException is not null)
            throw response.OriginalException;
        
        _logger.LogElasticIndexCreated(_indexNames.Places);
    }

    
    private DeleteIndexResponse RemoveIndex(string index)
        => Client.Indices.Delete(index);
    
    
    private CreateIndexResponse CreatePlacesIndex()
    {
        const string removeHouseNumberSuffixCharFilter = "remove_ws_hnr_suffix";
        const string removeHouseNumberSuffixPattern = @"(\d+)\s(?=\p{L}\b)";
        const string removePunctuationCharFilter = "remove_punctuation";
        const string removePunctuationPattern = @"[\.,']";
        const string edgeNgramTokenizer = "edge_ngram";
        const string indexEngramAnalyzer = "index_engram";
        const string searchEngramAnalyzer = "search_engram";
        const string indexRawAnalyzer = "index_raw";
        const string searchRawAnalyzer = "search_raw";
        const string indexHouseNumberAnalyzer = "index_housenumber";
        const string indexCountryEngramAnalyzer = "index_country_engram";
        const string indexCityEngramAnalyzer = "index_city_engram";
        const string collectorEn = "collector.en";
        const string collectorDe = "collector.de";
        const string collectorFr = "collector.fr";
        const string collectorDefault = "collector.default";
        const string countryTokenFilter = "country_synonyms";
        const string cityTokenFilter = "city_synonyms";
        
        var countrySynonyms = ElasticSynonymsHelper.GetCountrySynonyms();
        var citySynonyms = ElasticSynonymsHelper.GetLocalitySynonyms();
        
        return Client.Indices.Create(_indexNames.Places, ind
            => ind.Settings(isd => isd.Analysis(ad =>
                    ad.CharFilters(cfd => cfd
                            .PatternReplace(removeHouseNumberSuffixCharFilter,
                                prf => prf.Pattern(removeHouseNumberSuffixPattern).Replacement("$1"))
                            .PatternReplace(removePunctuationCharFilter,
                                prf => prf.Pattern(removePunctuationPattern).Replacement(" ")))
                        .Tokenizers(t => t.EdgeNGram(edgeNgramTokenizer,
                            et => et.MinGram(1).MaxGram(100).TokenChars(TokenChar.Digit, TokenChar.Letter)))
                        .TokenFilters(tf => tf
                            .SynonymGraph(countryTokenFilter, sgtf => sgtf.Lenient(false).Synonyms(countrySynonyms))
                            .SynonymGraph(cityTokenFilter, sgtf => sgtf.Lenient(false).Synonyms(citySynonyms)))
                        .Analyzers(a => a
                            .Custom(indexEngramAnalyzer,
                                ca => ca.CharFilters(removePunctuationCharFilter).Filters("word_delimiter", "lowercase",
                                    "german_normalization", "asciifolding", "unique").Tokenizer(edgeNgramTokenizer))
                            .Custom(searchEngramAnalyzer,
                                ca => ca.CharFilters(removePunctuationCharFilter)
                                    .Filters("lowercase", "german_normalization", "asciifolding").Tokenizer("standard"))
                            .Custom(indexRawAnalyzer,
                                ca => ca.CharFilters(removePunctuationCharFilter).Filters("word_delimiter", "lowercase",
                                    "german_normalization", "asciifolding", "unique").Tokenizer("standard"))
                            .Custom(searchRawAnalyzer,
                                ca => ca.CharFilters(removePunctuationCharFilter)
                                    .Filters("german_normalization", "asciifolding", "unique", "lowercase", "word_delimiter").Tokenizer("standard"))
                            .Custom(indexHouseNumberAnalyzer,
                                ca => ca.CharFilters(removePunctuationCharFilter, removeHouseNumberSuffixCharFilter)
                                    .Filters("lowercase", "word_delimiter").Tokenizer("standard"))
                            .Custom(indexCountryEngramAnalyzer,
                                ca => ca.CharFilters(removePunctuationCharFilter)
                                    .Filters("german_normalization", "asciifolding", "unique", "lowercase", countryTokenFilter).Tokenizer("standard"))
                            .Custom(indexCityEngramAnalyzer,
                                ca => ca.CharFilters(removePunctuationCharFilter)
                                    .Filters("german_normalization", "asciifolding", "unique", "lowercase", cityTokenFilter).Tokenizer("standard")))))
                .Map<Place>(tm =>
                    tm.Dynamic(false).Properties(p => p
                        .Object<MultiLanguageName>(DefineNameSearchProperties)
                        .Object<MultiLanguage>(np => np.Name(op=> op.Country)
                            .Properties(pr => DefineMultiLangEngramSearchProperties(pr, indexCountryEngramAnalyzer, searchEngramAnalyzer)))
                        .Object<MultiLanguage>(np => np.Name(op=> op.City)
                            .Properties(pr => DefineMultiLangEngramSearchProperties(pr, indexCityEngramAnalyzer, searchEngramAnalyzer)))
                        .Object<MultiLanguage>(np => np.Name(op=> op.County)
                            .Properties(pr => DefineMultiLangEngramSearchProperties(pr, indexEngramAnalyzer, searchEngramAnalyzer)))
                        .Object<MultiLanguage>(np => np.Name(op=> op.District)
                            .Properties(pr => DefineMultiLangEngramSearchProperties(pr, indexEngramAnalyzer, searchEngramAnalyzer)))
                        .Object<MultiLanguage>(np => np.Name(op=> op.Locality)
                            .Properties(pr => DefineMultiLangEngramSearchProperties(pr, indexEngramAnalyzer, searchEngramAnalyzer)))
                        .Object<MultiLanguage>(np => np.Name(op => op.Collector)
                            .Properties(pr => DefineMultiLangEngramSearchProperties(pr, indexRawAnalyzer, searchRawAnalyzer)))
                        .Object<MultiLanguage>(np => np.Name(op => op.State)
                            .Properties(pr => DefineMultiLangEngramSearchProperties(pr, indexRawAnalyzer, searchRawAnalyzer)))
                        .Object<MultiLanguage>(np => np.Name(op => op.Street)
                            .Properties(pr => DefineMultiLangEngramSearchProperties(pr, indexRawAnalyzer, searchRawAnalyzer)))
                        .Text(tp => tp.Name(op => op.HouseNumber).Analyzer(indexHouseNumberAnalyzer).SearchAnalyzer("standard"))
                        .Text(tp => tp.Name(op => op.Postcode).Index(false))
                        .Scalar(pl => pl.OsmId)
                        .GeoPoint(pp => pp.Name(op => op.Coordinate))
                        .GeoShape(sp => sp.Name(op => op.Extent))
                        .Keyword(tp => tp.Name(op => op.CountryCode))
                        .Number(np => np.Name(op => op.Importance))
                        .Keyword(kp => kp.Name(op => op.ObjectType))
                        .Keyword(pl => pl.Name(pl => pl.OsmKey))
                        .Keyword(pl => pl.Name(pl => pl.OsmType))
                        .Keyword(pl => pl.Name(pl => pl.OsmValue))
                    )));
                    
        
        PropertiesDescriptor<MultiLanguage> DefineMultiLangEngramSearchProperties(PropertiesDescriptor<MultiLanguage> pr, string indexAnalyzer, string searchAnalyzer) => 
            pr.Text(pd => pd.Name(op => op.En).Analyzer(indexAnalyzer)
                    .SearchAnalyzer(searchAnalyzer).CopyTo(f => f.Field(collectorEn)))
                .Text(pd => pd.Name(op => op.Default).Analyzer(indexAnalyzer)
                    .SearchAnalyzer(searchAnalyzer).CopyTo(f => f.Field(collectorDefault)))
                .Text(pd => pd.Name(op => op.Fr).Index(false).CopyTo(f => f.Field(collectorFr)))
                .Text(pd => pd.Name(op => op.De).Index(false).CopyTo(f => f.Field(collectorDe)));

        
        ObjectTypeDescriptor<Place, MultiLanguageName> DefineNameSearchProperties(ObjectTypeDescriptor<Place, MultiLanguageName> pr) =>
            pr.Name(td => td.Name)
                .Properties(p => p
                    .Text(pd => pd.Name(op => op.En).Analyzer(indexRawAnalyzer)
                        .SearchAnalyzer(searchRawAnalyzer).CopyTo(f => f.Field(collectorEn)))
                    .Text(pd => pd.Name(op => op.Default).Analyzer(indexRawAnalyzer)
                        .SearchAnalyzer(searchRawAnalyzer).CopyTo(f => f.Field(collectorDefault)))
                    .Text(pd => pd.Name(op => op.Fr).Index(false).CopyTo(f => f.Field(collectorFr)))
                    .Text(pd => pd.Name(op => op.De).Index(false).CopyTo(f => f.Field(collectorDe)))
                    .Text(pd => pd.Name(op => op.Alt).Index(false).CopyTo(f => f.Field(collectorDefault)))
                    .Text(pd => pd.Name(op => op.Int).Index(false).CopyTo(f => f.Field(collectorDefault)))
                    .Text(pd => pd.Name(op => op.Loc).Index(false).CopyTo(f => f.Field(collectorDefault)))
                    .Text(pd => pd.Name(op => op.Old).Index(false).CopyTo(f => f.Field(collectorDefault)))
                    .Text(pd => pd.Name(op => op.Reg).Index(false).CopyTo(f => f.Field(collectorDefault)))
                    .Text(pd => pd.Name(op => op.HouseName).Index(false).CopyTo(f => f.Field(collectorDefault))));
    }


    public IElasticClient Client { get; }
    private readonly IndexNames _indexNames;
    private readonly ILogger<GeoServiceElasticClient> _logger;
}