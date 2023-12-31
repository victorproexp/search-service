using SearchApi.Models;

namespace SearchApi
{
    public class SearchLogic : ISearchLogic
    {
        private readonly IDatabase database;
        private readonly IWordManager wordManager;
        public bool IsNormalized { get; private set; }

        public SearchLogic(IDatabase database, IWordManager wordManager)
        {
            this.database = database;
            this.wordManager = wordManager;
            IsNormalized = wordManager is WordManager<List<int>>;
        }

        public SearchResult Search(string[] query, int maxAmount)
        {
            query = NormalizeQueryIfNecessary(query);
            
            var wordIds = wordManager.GetWordIds(query, out List<string> ignored);

            var docIds = database.GetDocuments(wordIds);

            var top = docIds.Take(Math.Min(maxAmount, docIds.Count))
                          .Select(p => p.Key)
                          .ToList();

            var docDetails = database.GetDocDetails(top);
            var docResults = docDetails.Select((doc, index) => new DocumentHit(doc, docIds[index].Value))
                                      .ToList();

            return new SearchResult(query, docIds.Count, docResults, ignored);
        }

        private string[] NormalizeQueryIfNecessary(string[] query)
        {
            if (IsNormalized)
            {
                return query.Select(q => q.ToLower()).ToArray();
            }
            return query;
        }
    }
}
