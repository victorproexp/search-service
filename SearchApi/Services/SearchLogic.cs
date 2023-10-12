using SearchApi.Database;
using SearchApi.Models;

namespace SearchApi.Services
{
    public class SearchLogic : ISearchLogic
    {
        private readonly IDatabase mDatabase;
        private readonly IWordManager mWordManager;

        public SearchLogic(IDatabase database)
        {
            mDatabase = database;
            mWordManager = new WordManager(mDatabase.GetAllWords());
        }

        public SearchResult Search(string[] query, int maxAmount)
        {
            DateTime start = DateTime.Now;
            
            var wordIds = mWordManager.GetWordIds(query, out List<string> ignored);

            var docIds = mDatabase.GetDocuments(wordIds);

            var top = docIds.Take(Math.Min(maxAmount, docIds.Count))
                          .Select(p => p.Key)
                          .ToList();

            var docDetails = mDatabase.GetDocDetails(top);
            var docResults = docDetails.Select((doc, idx) => new DocumentHit(doc, docIds[idx].Value))
                                      .ToList();

            return new SearchResult(query, docIds.Count, docResults, ignored, DateTime.Now - start);
        }
    }
}
