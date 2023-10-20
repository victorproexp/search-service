using SearchApi.Models;

namespace SearchApi
{
    public interface IDatabase
    {
        // Dictionary<string, List<int>> GetAllWordsNormalized();
        Dictionary<string, int> GetAllWords();
        List<BEDocument> GetDocDetails(List<int> docIds);
        List<KeyValuePair<int, int>> GetDocuments(List<int> wordIds);
    }
}