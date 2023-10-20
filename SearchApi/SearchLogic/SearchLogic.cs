using SearchApi.Models;

namespace SearchApi
{
    public class SearchLogic : ISearchLogic
    {
        private readonly IDatabase database;
        private readonly IWordManager wordManager;

        public SearchLogic(IWordManager wordManager)
        {
            database = new Database();
            this.wordManager = wordManager;
        }

        public SearchResult Search(string[] query, int maxAmount)
        {
            if (wordManager is WordManager<List<int>>)
            {
                query = query.Select(q => q.ToLower()).ToArray();
            }

            DateTime start = DateTime.Now;
            
            var wordIds = wordManager.GetWordIds(query, out List<string> ignored);

            var docIds = database.GetDocuments(wordIds);

            var top = docIds.Take(Math.Min(maxAmount, docIds.Count))
                          .Select(p => p.Key)
                          .ToList();

            var docDetails = database.GetDocDetails(top);
            var docResults = docDetails.Select((doc, idx) => new DocumentHit(doc, docIds[idx].Value))
                                      .ToList();

            return new SearchResult(query, docIds.Count, docResults, ignored, DateTime.Now - start);
        }
    }
}
