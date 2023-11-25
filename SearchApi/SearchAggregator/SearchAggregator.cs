using SearchApi.Models;

namespace SearchApi
{
    public class SearchAggregator : ISearchAggregator
    {
        const int DefaultMaxAmount = 10;
        readonly List<ISearchLogic> searchLogics = new();

        public SearchAggregator()
        {
            InitializeSearchLogics();
        }

        public async Task<SearchResult> GetSearchResult(string query, int? maxAmount) =>
            await AggregateSearchResults(GetNonNormalizedLogics(), query, maxAmount);

        public async Task<SearchResult> GetNormalizedSearchResult(string query, int? maxAmount) =>
            await AggregateSearchResults(GetNormalizedLogics(), query, maxAmount);

        private static async Task<SearchResult> AggregateSearchResults(IEnumerable<ISearchLogic> searchLogics, string query, int? maxAmount)
        {
            var startTime = DateTime.Now;
            
            var searchTasks = CreateSearchTasks(searchLogics, query, maxAmount);

            var searchResults = await Task.WhenAll(searchTasks);

            var searchResult = MergeSearchResults(searchResults);

            searchResult.TimeUsed = DateTime.Now - startTime;

            return searchResult;
        }

        private static List<Task<SearchResult>> CreateSearchTasks(IEnumerable<ISearchLogic> searchLogics, string query, int? maxAmount)
        {
            var tasks = new List<Task<SearchResult>>();
            foreach (var searchLogic in searchLogics)
            {
                tasks.Add(Task.Run(() => ExecuteSearch(searchLogic, query, maxAmount)));
            }
            return tasks;
        }

        private static SearchResult ExecuteSearch(ISearchLogic searchLogic, string query, int? maxAmount)
        {
            try
            {
                return searchLogic.Search(query.Split(','), maxAmount ?? DefaultMaxAmount);
            }
            catch (Exception)
            {
                Console.WriteLine($"Search failed with query: {query}");
                throw;
            }
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
