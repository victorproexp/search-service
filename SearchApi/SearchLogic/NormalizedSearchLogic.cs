using SearchApi.Models;

namespace SearchApi
{
    public class NormalizedSearchLogic : ISearchLogic
    {
        private readonly NormalizedWordDictionary dictionary;
        private readonly IWordManager wordManager;

        public NormalizedSearchLogic(NormalizedWordDictionary dictionary)
        {
            this.dictionary = dictionary;
            wordManager = new NormalizedWordManager(dictionary.GetAllWords());
        }

        public SearchResult Search(string[] query, int maxAmount)
        {
            DateTime start = DateTime.Now;

            var normalizedQuery = query.Select(q => q.ToLower()).ToArray();
            var wordIds = wordManager.GetWordIds(normalizedQuery, out List<string> ignored);

            var docIds = dictionary.database.GetDocuments(wordIds);

            var top = docIds.Take(Math.Min(maxAmount, docIds.Count))
                        .Select(p => p.Key)
                        .ToList();

            var docDetails = dictionary.database.GetDocDetails(top);
            var docResults = docDetails.Select((doc, idx) => new DocumentHit(doc, docIds[idx].Value))
                                    .ToList();

            return new SearchResult(query, docIds.Count, docResults, ignored, DateTime.Now - start);
        }
    }
}
