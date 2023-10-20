using SearchApi.Models;

namespace SearchApi
{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, int maxAmount);
    }
}