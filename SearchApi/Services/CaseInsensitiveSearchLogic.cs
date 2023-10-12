using SearchApi.Models;
using SearchApi.Database;

namespace SearchApi.Services
{
    public class CaseInsensitiveSearchLogic : ISearchLogic
    {
        readonly CaseInsensitivePostgresDatabase mDatabase;

        // a cache for all words in the documents
        readonly Dictionary<string, List<int>> mWords;

        public CaseInsensitiveSearchLogic()
        {
            mDatabase = new CaseInsensitivePostgresDatabase();
            mWords = mDatabase.GetAllWordsCaseInsensitive();
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about up to maxAmount of documents.
         */
        public SearchResult Search(string[] query, int maxAmount)
        {
            DateTime start = DateTime.Now;

            // Normalize query words to lowercase for case-insensitive matching
            var normalizedQuery = query.Select(q => q.ToLower()).ToArray();

            // Convert words to wordids
            var wordIds = GetWordIds(normalizedQuery, out List<string> ignored);

            // perform the search - get all docIds
            var docIds = mDatabase.postgresDatabase.GetDocuments(wordIds);

            // get ids for the first maxAmount             
            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(maxAmount, docIds.Count)))
                top.Add(p.Key);

            // compose the result.
            // all the documentHit
            List<DocumentHit> docresult = new();
            int idx = 0;
            foreach (var doc in mDatabase.postgresDatabase.GetDocDetails(top))
                docresult.Add(new DocumentHit(doc, docIds[idx++].Value));

            return new SearchResult(query, docIds.Count, docresult, ignored, DateTime.Now - start);
        }

        private List<int> GetWordIds(string[] query, out List<string> outIgnored)
        {
            var res = new List<int>();
            var ignored = new List<string>();

            foreach (var aWord in query)
            {
                if (mWords.ContainsKey(aWord))
                    res.AddRange(mWords[aWord]); // Handle duplicates
                else
                    ignored.Add(aWord);
            }
            outIgnored = ignored;
            return res;
        }
    }
}
