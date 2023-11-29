using SearchApi.Models;

namespace SearchApi
{
    public static class SearchExecutor
    {
        public static async Task<SearchResult> SearchAsync(IEnumerable<ISearchLogic> searchLogics, SearchParameters parameters)
        {
            var startTime = DateTime.Now;
            var searchTasks = CreateSearchTasks(searchLogics, parameters);
            var searchResults = await Task.WhenAll(searchTasks);
            var searchResult = MergeSearchResults(searchResults);
            searchResult.TimeUsed = DateTime.Now - startTime;

            return searchResult;
        }

        private static List<Task<SearchResult>> CreateSearchTasks(IEnumerable<ISearchLogic> searchLogics, SearchParameters parameters)
        {
            var tasks = new List<Task<SearchResult>>();
            foreach (var searchLogic in searchLogics)
            {
                tasks.Add(Task.Run(() => PerformSingleSearch(searchLogic, parameters)));
            }
            return tasks;
        }

        private static SearchResult PerformSingleSearch(ISearchLogic searchLogic, SearchParameters parameters)
        {
            try
            {
                return searchLogic.Search(parameters.Query.Split(','), parameters.MaxAmount);
            }
            catch (Exception)
            {
                Console.WriteLine($"SearchLogic failed with query: {parameters.Query}");
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
    }
}