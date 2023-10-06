using System;
namespace WebSearch
{
    /**
     * A class used to represent a document together with a
     * counter for how many words from a query that is in 
     * the document.
     * */
    public class DocumentHit
    {
        public DocumentHit(BEDocument doc, int noOfHits)
        {
            Document = doc;
            NoOfHits = noOfHits;
        }

        public BEDocument Document { get; set; }

        public int NoOfHits { get; set; }
    }
}

