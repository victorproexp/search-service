using SearchApi.Models;

namespace SearchApi
{
    public class SearchAggregator : ISearchAggregator
    {
        private readonly List<ISearchLogic> searchLogics = new();
        private readonly ISearchCache searchCache;
        
        public SearchAggregator(ISearchCache searchCache)
        {
            this.searchCache = searchCache;

            var databases = new List<IDatabase>
            {
                new Database("postgres"),
                new Database("postgresadd")
            };

            searchLogics.AddRange(databases.SelectMany(SearchFactory.CreateSearchLogics));
        }

        public async Task<SearchResult> GetSearchResult(string query, int? maxAmount) =>
            await AggregateSearchResults(GetNonNormalizedLogics(), new SearchParameters(false, query, maxAmount));

        public async Task<SearchResult> GetNormalizedSearchResult(string query, int? maxAmount) =>
            await AggregateSearchResults(GetNormalizedLogics(), new SearchParameters(true, query, maxAmount));

        private async Task<SearchResult> AggregateSearchResults(IEnumerable<ISearchLogic> searchLogics, SearchParameters parameters)
        {
            var cacheKey = Utils.CreateCacheKey(parameters);
            var cachedResult = searchCache.GetCachedResult(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }
            
            var searchResult = await SearchExecutor.SearchAsync(searchLogics, parameters);
            searchCache.CacheResult(cacheKey, searchResult);

            return searchResult;
        }

        private IEnumerable<ISearchLogic> GetNonNormalizedLogics() =>
            searchLogics.Where(logic => !logic.IsNormalized);

        private IEnumerable<ISearchLogic> GetNormalizedLogics() =>
            searchLogics.Where(logic => logic.IsNormalized);
    }
}
