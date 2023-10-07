using Shared.BE;

namespace ConsoleSearch
{
    public interface IDatabase
    {
        Dictionary<string, int> GetAllWords();
        List<BEDocument> GetDocDetails(List<int> docIds);
        List<KeyValuePair<int, int>> GetDocuments(List<int> wordIds);
    }
}