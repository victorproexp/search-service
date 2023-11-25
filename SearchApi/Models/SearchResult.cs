namespace SearchApi.Models
{
    /*
     * A data class representing the result of a search.
     * Hits is the total number of documents containing at least one word from the query.
     * DocumentHits is the documents and the number of words from the query contained in the document - see
     * the class DocumentHit
     * Ignored contains words from the query not present in the document base.
     * TimeUsed is the timespan used to perform the search.
     */
    public class SearchResult
    {
        public SearchResult(String[] query, int hits, List<DocumentHit> documents, List<string> ignored)
        {
            Query = query;
            Hits = hits;
            DocumentHits = documents;
            Ignored = ignored;
            TimeUsed = default;
        }

        public String[] Query { get; private set; }

        public int Hits { get; private set; }

        public List<DocumentHit> DocumentHits { get; private  set; }

        public List<string> Ignored { get; private set; }

        public TimeSpan TimeUsed { get;  set; }
    }
}
