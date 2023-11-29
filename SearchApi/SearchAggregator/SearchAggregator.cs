using SearchApi.Models;

namespace SearchApi
{
    public class SearchAggregator : ISearchAggregator
    {
        private readonly List<ISearchLogic> searchLogics = new();
        private readonly SearchCache searchCache = new();
        
        public SearchAggregator()
        {
            InitializeSearchLogics();
        }

        public async Task<SearchResult> GetSearchResult(string query, int? maxAmount) =>
            await AggregateSearchResults(GetNonNormalizedLogics(), new SearchParameters(query, maxAmount));

        public async Task<SearchResult> GetNormalizedSearchResult(string query, int? maxAmount) =>
            await AggregateSearchResults(GetNormalizedLogics(), new SearchParameters(query, maxAmount));

        private void InitializeSearchLogics()
        {
            var databases = new List<IDatabase>
            {
                new Database("postgres"),
                new Database("postgresadd")
            };

            foreach (var database in databases)
            {
                searchLogics.AddRange(SearchFactory.CreateSearchLogics(database));
            }
        }

        private async Task<SearchResult> AggregateSearchResults(IEnumerable<ISearchLogic> searchLogics, SearchParameters parameters)
        {
            var cacheKey = SearchCache.CreateCacheKey(parameters);
            var cachedResult = searchCache.GetCachedResult(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var searchResult = await SearchExecutor.SearchAsync(searchLogics, parameters);
            searchCache.CacheResult(cacheKey, searchResult);

            return searchResult;
        }

        private IEnumerable<ISearchLogic> GetNonNormalizedLogics()
        {
            return searchLogics.Where(logic => !logic.IsNormalized);
        }

        private IEnumerable<ISearchLogic> GetNormalizedLogics()
        {
            return searchLogics.Where(logic => logic.IsNormalized);
        }
    }
}
