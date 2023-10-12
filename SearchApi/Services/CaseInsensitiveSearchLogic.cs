using SearchApi.Models;
using SearchApi.Database;

namespace SearchApi.Services
{
    public class CaseInsensitiveSearchLogic : ISearchLogic
    {
        private readonly CaseInsensitivePostgresDatabase mDatabase;
        private readonly IWordManager mWordManager;

        public CaseInsensitiveSearchLogic(CaseInsensitivePostgresDatabase caseInsensitivePostgresDatabase)
        {
            mDatabase = caseInsensitivePostgresDatabase;
            mWordManager = new CaseInsensitiveWordManager(mDatabase.GetAllWordsCaseInsensitive());
        }

        public SearchResult Search(string[] query, int maxAmount)
        {
            DateTime start = DateTime.Now;

            var normalizedQuery = query.Select(q => q.ToLower()).ToArray();
            var wordIds = mWordManager.GetWordIds(normalizedQuery, out List<string> ignored);

            var docIds = mDatabase.postgresDatabase.GetDocuments(wordIds);

            var top = docIds.Take(Math.Min(maxAmount, docIds.Count))
                        .Select(p => p.Key)
                        .ToList();

            var docDetails = mDatabase.postgresDatabase.GetDocDetails(top);
            var docResults = docDetails.Select((doc, idx) => new DocumentHit(doc, docIds[idx].Value))
                                    .ToList();

            return new SearchResult(query, docIds.Count, docResults, ignored, DateTime.Now - start);
        }
    }
}
