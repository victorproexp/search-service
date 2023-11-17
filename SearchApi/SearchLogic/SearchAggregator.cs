using SearchApi.Models;

namespace SearchApi
{
    public class SearchAggregator : ISearchAggregator
    {
        const int DefaultMaxAmount = 10;
        readonly List<ISearchLogic> searchLogics = new();
        readonly List<ISearchLogic> normalizedSearchLogics = new();

        public SearchAggregator()
        {
            InitializeSearchLogics();
        }

        public async Task<SearchResult> GetSearchResult(string query, int? maxAmount) =>
            await AggregateSearchResults(searchLogics, query, maxAmount);

        public async Task<SearchResult> GetNormalizedSearchResult(string query, int? maxAmount) => 
            await AggregateSearchResults(normalizedSearchLogics, query, maxAmount);

        private static async Task<SearchResult> AggregateSearchResults(List<ISearchLogic> searchLogics, string query, int? maxAmount)
        {
            DateTime start = DateTime.Now;
            
            var searchTasks = CreateSearchTasks(searchLogics, query, maxAmount);

            var searchResults = await Task.WhenAll(searchTasks);

            var searchResult = MergeSearchResults(searchResults);

            searchResult.TimeUsed = DateTime.Now - start;

            return searchResult;
        }

        private static List<Task<SearchResult>> CreateSearchTasks(List<ISearchLogic> searchLogics, string query, int? maxAmount)
        {
            var tasks = new List<Task<SearchResult>>();
            foreach (var searchLogic in searchLogics)
            {
                tasks.Add(Task.Run(() => searchLogic.CreateSearchResult(
                    query.Split(','), 
                    maxAmount ?? DefaultMaxAmount
                )));
            }
            return tasks;
        }

        private static SearchResult MergeSearchResults(SearchResult[] searchResults)
        {
            var mergedQuery = searchResults.SelectMany(sr => sr.Query).Distinct().ToArray();

            var totalHits = searchResults.Sum(sr => sr.Hits);

            var mergedDocumentHits = searchResults.SelectMany(sr => sr.DocumentHits).ToList();

            var mergedIgnored = searchResults.SelectMany(sr => sr.Ignored).Distinct().ToList();

            return new SearchResult(mergedQuery, totalHits, mergedDocumentHits, mergedIgnored);
        }

        private void InitializeSearchLogics()
        {
            List<IDatabase> databases = new() {
                new Database("postgres"),
                new Database("postgresadd")
            };

            foreach (var database in databases)
            {
                searchLogics.Add(SearchFactory.CreateSearchLogic(database));
                normalizedSearchLogics.Add(SearchFactory.CreateNormalizedSearchLogic(database));
            }
        }
    }
}
