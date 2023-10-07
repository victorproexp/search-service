namespace ConsoleSearch
{
    public class SearchLogic : ISearchLogic
    {
        readonly IDatabase mDatabase;

        // a cache for all words in the documents
        readonly Dictionary<string, int> mWords;

        public SearchLogic()
        {
            mDatabase = new PostgresDatabase();
            mWords = mDatabase.GetAllWords();
            Console.WriteLine(mWords.First());
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about up to maxAmount of documents.
         */
        public SearchResult Search(String[] query, int maxAmount)
        {
            DateTime start = DateTime.Now;

            // Convert words to wordids
            var wordIds = GetWordIds(query, out List<string> ignored);

            // perform the search - get all docIds
            var docIds = mDatabase.GetDocuments(wordIds);

            // get ids for the first maxAmount             
            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(maxAmount, docIds.Count)))
                top.Add(p.Key);

            // compose the result.
            // all the documentHit
            List<DocumentHit> docresult = new List<DocumentHit>();
            int idx = 0;
            foreach (var doc in mDatabase.GetDocDetails(top))
                docresult.Add(new DocumentHit(doc, docIds[idx++].Value));


            return new SearchResult(query, docIds.Count, docresult, ignored, DateTime.Now - start);
        }

        private List<int> GetWordIds(String[] query, out List<string> outIgnored)
        {
            var res = new List<int>();
            var ignored = new List<string>();

            foreach (var aWord in query)
            {
                if (mWords.ContainsKey(aWord))
                    res.Add(mWords[aWord]);
                else
                    ignored.Add(aWord);
            }
            outIgnored = ignored;
            return res;
        }


    }
}
