using SearchApi.Models;

namespace SearchApi.Services
{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, int maxAmount);
    }
}