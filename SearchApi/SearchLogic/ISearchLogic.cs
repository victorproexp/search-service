using SearchApi.Models;

namespace SearchApi
{
    public interface ISearchLogic
    {
        SearchResult CreateSearchResult(string[] query, int maxAmount);
    }
}