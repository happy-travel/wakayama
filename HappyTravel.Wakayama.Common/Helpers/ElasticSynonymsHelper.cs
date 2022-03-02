namespace HappyTravel.Wakayama.Common.Helpers;

public static class ElasticSynonymsHelper
    {
        public static IEnumerable<string> GetAllSynonyms() 
            => GetCountrySynonyms().Concat(GetLocalitySynonyms());
        

        public static IEnumerable<string> GetCountrySynonyms()
        {
            var countries = LocationNameRetriever.RetrieveCountries();
            
            return countries.Select(c => FilterSynonyms(c.Name.Variants))
                .Where(IsNotOneWordList)
                .Select(CreateSynonym);
        }

        
        public static IEnumerable<string> GetLocalitySynonyms()
        {
            var countries = LocationNameRetriever.RetrieveCountries();
            
            return countries
                .Where(c => c.Localities != null)
                .SelectMany(c => c.Localities)
                .Select(l => FilterSynonyms(l.Name.Variants))
                .Where(IsNotOneWordList)
                .Select(CreateSynonym);
        }
        
        
        private static IEnumerable<string> FilterSynonyms(IEnumerable<string> names)
            => names.Select(RemoveArticles).Distinct();

        
        private static bool IsNotOneWordList(IEnumerable<string> synonyms) => synonyms.Count() > 1;
        
        
        private static string RemoveArticles(string origin)
        {
            foreach (var article in Articles)
            {
                if (origin.Length <= article.Length)
                    return origin;
            
                var originArticle = origin.Substring(0, article.Length).ToLowerInvariant();
            
                if (originArticle.Equals(article))
                    return origin.Substring(article.Length + 1);
            }

            return origin;
        }
        
        
        private static string CreateSynonym(IEnumerable<string> names) 
            => $"{string.Join(", ", names.Skip(1))} => {names.First()}".ToLowerInvariant();

        
        private static LocationNameNormalizer.FileLocationNameRetriever LocationNameRetriever => new();
        
        private static readonly List<string> Articles = new() {"the", "a", "an"};
    }